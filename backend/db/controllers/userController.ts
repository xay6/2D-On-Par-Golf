import { Request, Response } from "express";
import User, { IUser } from "../models/User";
const bcrypt = require("bcrypt");

const hashPw = async (password: String) => {
    return await bcrypt.hash(password, 12);
}

export const register = async (req: Request, res: Response): Promise<void> => {
    try {
        if (!req.body.username) {
            res.status(400);
            throw new Error("Enter a Username.")
        } else if (!req.body.password) {
            res.status(400);
            throw new Error("Enter a Password.")
        }

        const username = req.body.username;
        const userExists = await User.findOne({ username })
            .catch(() => {
                res.status(500);
                throw new Error("Internal server error.");
            });

        if (userExists) {
            res.send("Username is already taken.");
            return;
        }
        
        const hashedPw = await hashPw(req.body.password);
        const newUser: IUser = new User({
            username,
            password: hashedPw,
        })
        await newUser.save();

        res.status(201).json({ message: 'User created successfully' });
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