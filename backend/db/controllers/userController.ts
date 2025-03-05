import { Request, Response } from "express";
import User, { IUser } from "../models/User";
const bcrypt = require("bcrypt");

const hashPw = async (password: String) => {
    return await bcrypt.hash(password, 12);
}

export const register = async (req: Request, res: Response): Promise<void> => {
    try {
        if (!req.body.username && !req.body.password) {
            res.status(422);
            throw new Error("Username and password empty.")
        } else if (!req.body.username) {
            res.status(422);
            throw new Error("Username empty.")
        } else if (!req.body.password) {
            res.status(422);
            throw new Error("Password empty.")
        }

        const username = req.body.username;
        const userExists = await User.findOne({ username })
            .catch(() => {
                res.status(500);
                throw new Error("Internal server error.");
            });

        if (userExists) {
            res.status(422);
            throw new Error("Username is already taken.");
        } else if (!username.match(/^[a-zA-Z1-9]\w{6,12}$/gm)) {
            res.status(422);
            throw new Error("Username must be 6 - 12 characters long, start with a letter or number, and can only contain letters, numbers, and _.");
        } else if(!req.body.password.match(/^\w{6,}$/gm)) {
            res.status(422);
            throw new Error("Password must be at least 6 characters long.");
        }
        
        const hashedPw = await hashPw(req.body.password);
        const newUser: IUser = new User({
            username,
            password: hashedPw,
        })
        await newUser.save();

        res.status(201).json({ message: "User account created." });
    } catch (err: any) {
        res.json({ message: err.message });
        console.error(err.message);
    }
}

export const login = async (req: Request, res: Response): Promise<void> => {
    try {
        const { username, password } = req.body;

        const userLogin = await User.findOne({ username })
            .then((result) => {
                if (password === result?.password) {
                    res.status(200);
                    res.json({ message: 'Login Successfull' });
                } else {
                    res.json({ message: "Invalid login credentials." })
                }
            })
            .catch(() => { throw new Error("Internal server error.") });

    res.status(201).json({ message: 'User successfully logged in.' });
    } catch (err: any) {
        res.status(500).json({ message: "Server Error. Try again later." });
        console.error(err.message);
    }
}