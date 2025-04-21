import { NextFunction, Request, Response } from "express";
import User, { IUser } from "../models/User";
import { AuthenticatedRequest } from "../types";
import { authenticateJwt } from "../../middleware/authenticateJwt";
import CourseScores from "../models/CourseScores";
import getRedisClient from "../redis";
import { checkTokenBlacklist } from "../../middleware/checkTokenBlacklist";
const bcrypt = require("bcrypt");
const jsonwebtoken = require('jsonwebtoken');
import * as crypto from 'crypto';

const hashPw = async (password: string) => {
    return await bcrypt.hash(password, 12);
}
/*
    Error Messages:
        Username and password are empty.
        Username is empty.
        Password is empty.
        Username is already taken.
        Username must be 6 - 12 characters long, start with a letter or number, and can only contain letters, numbers, and _.
        Password must be at least 6 characters long.
*/
export const register = async (req: Request, res: Response): Promise<void> => {
    try {
        if (!req.body.username && !req.body.password) {
            res.status(422);
            res.json({ message: "Username and password are empty.", success: false });
            return;
        } else if (!req.body.username) {
            res.status(422);
            res.json({ message: "Username is empty.", success: false });
            return;
        } else if (!req.body.password) {
            res.status(422);
            res.json({ message: "Password is empty.", success: false });
            return;
        }

        const username = req.body.username;
        const userExists = await User.findOne({ username })
            .catch((err: any) => {
                res.status(500);
                console.error(err.message);
                res.json({ message: "Internal server error.", success: false });
            });

        if (userExists) {
            res.status(422);
            res.json({ message: "Username is already taken.", success: false });
            return;
        } else if (!username.match(/^[a-zA-Z1-9]\w{3,12}$/gm)) {
            res.status(422);
            res.json({ message: "Username must be 4 - 12 characters long, start with a letter or number, and can only contain letters, numbers, and _.", success: false });
            return;
        } else if (!req.body.password.match(/^\w{5,}$/gm)) {
            res.status(422);
            res.json({ message: "Password must be at least 6 characters long.", success: false });
            return;
        }

        const hashedPw = await hashPw(req.body.password);
        const newUser: IUser = new User({
            username,
            password: hashedPw,
        })
        await newUser.save();

        console.log("User account created.");
        res.status(201).json({ message: "User account created.", success: true });
    } catch (err: any) {
        console.error("Error in register", err);
        res.status(500).json(`Error in register: ${err}`);
    }
}

/*
    Error Messages:
        Username is empty.
        Password is empty.
        User not found.
        Incorrect password. Try again.
*/
export const login = async (req: Request, res: Response): Promise<void> => {
    try {
        const username = req.body.username;

        if (!username) {
            res.status(422);
            res.json({ message: "Username is empty.", success: false });
            return;
        }

        if (!req.body.password) {
            res.status(422);
            res.json({ message: "Password is empty.", success: false });
            return;
        }

        const user = await User.findOne({ username });

        if (!user) {
            res.status(404).json({ message: 'User not found.', success: false });
            return;
        }

        const isValidPassword = await bcrypt.compare(req.body.password, user.password);

        if (!isValidPassword) {
            res.status(422).json({ message: "Incorrect password. Try again.", success: false });
            return;
        }

        const token = jsonwebtoken.sign({ username, id: user._id, exp: Math.floor(Date.now() / 1000) + (60 * 60 * 24 * 10) }, process.env.JWT_SECRET);
        res.status(201).json({ message: "Logging in.", token, success: true });
    } catch (err: any) {
        console.error("Error in login", err);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
    }
}

export const getUser = async (req: Request, res: Response): Promise<void> => {
    try {
        const { username } = req.body;
        const user = await User.findOne({ username });

        if (user) {
            res.status(200).json({ message: "User found.", user, success: true });
            console.log("User found: ", user);
        } else {
            console.log("User not found: ");
            res.status(404).json({ message: "User not found", success: false });
        }
    } catch (err: any) {
        console.log("getUser", err);
        res.status(400).json({ message: "An error occurred when fetching the user.", error: err.message, success: false });
    }
}

/*
    Error Messages:
        Username is empty.
        You are not authorized to delete this user.
        User not found.
*/
export const deleteUser = [authenticateJwt, checkTokenBlacklist, checkTokenBlacklist,
    (err: any, req: AuthenticatedRequest, res: Response, next: NextFunction) => { // Handle when a user is not authorized
        if (err.name === 'UnauthorizedError') {
          res.status(401).json({ message: 'Invalid token', success: false });
        } else {
          next();
        }
    },
async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { username } = req.body;

        if (!username) {
            res.status(422).json({ message: "Username is empty.", success: false });
            return;
        }

        const user = await User.findOne({ username }).exec();

        if (!user) {
            res.status(404).json({ message: "User not found", success: false });
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to delete this user." });
            return;
        }

        // Delete all of the user's scores from the database.
        await CourseScores.updateMany(
            { 'userData.user': user._id },
            { $pull: { userData: { user: user._id } } }
        );
        
        await user.deleteOne().exec();

        console.log("User found: ", user);
        res.status(200).json({ message: "User deleted.", user, success: true });
    } catch (err: any) {
        console.log("Error in deleteUser: ", err);
        res.status(500).json({ message: `An error occurred when deleting the user.${err.message}`, success: false });
    }
}]

/*
    Stores tokens in Redis which serves as a "blacklist" for users tokens to invalidate them after they log out of their accounts.

    POSSIBLE ROOM FOR IMPROVEMENT:
        Implement refresh and access tokens.
*/
const addToDenylist = async (token: string, expireTime: number) => {
    const redisClient = await getRedisClient();
    const expiresIn = Math.min(
        Math.floor(expireTime - Date.now() / 1000), // Seconds till JWT expiration
        10 * 24 * 60 * 60 // 10 days in seconds(Set expiration time for all JWTs)
    );
    await redisClient?.setEx(`bl_${token}`, expiresIn, '1');
};
export const logout = [authenticateJwt, checkTokenBlacklist,
    (err: any, req: AuthenticatedRequest, res: Response, next: NextFunction) => { // Handle when a user is not authorized
        if (err.name === 'UnauthorizedError') {
            res.status(401).json({ message: 'Invalid token', success: false });
        } else {
            next();
        }
    },
async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const authHeader = req.headers['authorization'];

        if(!authHeader || !authHeader.startsWith("Bearer ")) {
            console.log('Missing or invalid authorization header');
            res.status(401).json({ message: 'Missing or invalid authorization header' });
        }

        const hashedToken = crypto.createHash('sha256').update(authHeader?.split(' ')[1] as string).digest('hex'); // Hashed to reduce size
        const decodedToken = jsonwebtoken.decode(authHeader?.split(' ')[1] as string);
        await addToDenylist(hashedToken, decodedToken.exp);
        res.status(200).json({ message: "Successfully logged out.", success: true });
    } catch (err: any) {
        console.log(`Internal server error: ${err.message}`);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
    }
}]

export const validate = [authenticateJwt, checkTokenBlacklist, checkTokenBlacklist,checkTokenBlacklist,
    (err: any, req: AuthenticatedRequest, res: Response) => {
        try {
            if (err.name === 'UnauthorizedError') {
                console.log('User not logged in.');
                res.status(401).json({ message: 'User not logged in.', success: false });
            }
            res.status(200).json({ message: "Valid access token.", success: true });
        } catch (err: any) {
            console.log(`Internal server error: ${err.message}`);
            res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
        }
    }
]