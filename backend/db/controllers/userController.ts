import { Request, Response } from "express";
import User, { IUser } from "../models/User";
const bcrypt = require("bcrypt");

const hashPw = async (password: String) => {
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
            res.json({ message: "Username and password are empty.", success: false })
            return;
        } else if (!req.body.username) {
            res.status(422);
            res.json({ message: "Username is empty.", success: false })
            return;
        } else if (!req.body.password) {
            res.status(422);
            res.json({ message: "Password is empty.", success: false })
            return;
        }

        const username = req.body.username;
        const userExists = await User.findOne({ username })
            .catch((err: any) => {
                res.status(500);
                res.json({ message: "Internal server error.", success: false });
                console.error(err.message);
            });

        if (userExists) {
            res.status(422);
            res.json({ message: "Username is already taken.", success: false });
            return;
        } else if (!username.match(/^[a-zA-Z1-9]\w{6,12}$/gm)) {
            res.status(422);
            res.json({ message: "Username must be 6 - 12 characters long, start with a letter or number, and can only contain letters, numbers, and _.", success: false });
            return;
        } else if (!req.body.password.match(/^\w{6,}$/gm)) {
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

        res.status(201).json({ message: "User account created.", success: true });
    } catch (err: any) {
        console.error(err.message);
    }
}

/*
    Error Messages:
        Incorrect password. Try again.
*/
export const login = async (req: Request, res: Response): Promise<void> => {
    try {
        const username = req.body.username;

        const userLogin = await User.findOne({ username })
            .catch((err: any) => {
                res.status(500);
                console.error(err.message);
            });

        if (userLogin) {
            bcrypt.compare(req.body.password, userLogin.password,
                (err: any, result: boolean) => {
                    if (err) {
                        console.error(err.message);
                        return;
                    }

                    if (result) {
                        res.status(201).json({ message: "Logging in.", success: true });
                    } else {
                        res.status(422);
                        res.json({ message: "Incorrect password. Try again.", success: false });
                        return;
                    }
                }
            );
        } else {
            res.status(422);
            res.json({ message: "User not found.", success: false });
        }
    } catch (err: any) {
        console.error(err.message);
    }
}