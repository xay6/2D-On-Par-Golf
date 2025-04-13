import { Response } from "express";
import User from "../models/User";
import { AuthenticatedRequest } from "../types";
import UserItems from "../models/UserItems";
import { authenticateJwt } from "../../middleware/authenticateJwt";

export const updateCoins = [authenticateJwt, async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { username, coinAmount } = req.body;
        if (!username && isNaN(coinAmount)) {
            res.status(422).json({ message: "Invalid input.", success: false });
            return;
        }

        const user = await User.findOne({ username });

        if (!user) {
            res.status(404).json({ message: "User not found.", success: false });
            console.error("User not found.");
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to set edit items.", success: false });
            console.error("You are not authorized to edit items.");
            return;
        }

        const userItems = await UserItems.findOne({ user });
        if (!userItems) {
            const newUserItems = new UserItems({
                user,
                coinAmount: coinAmount || 0,
            });
            await newUserItems.save();
            res.status(200).json({ message: `Coins created for ${username}.`, success: true });
            console.log(`Coins created for ${username}.`);
            return;
        }

        if (coinAmount)
            await userItems.updateOne({
                coinAmount,
            });

        res.status(200).json({ message: `Coins changed for ${username}.`, success: true });
        console.log(`Coins changed for ${username}.`);

    } catch (err: any) {

    }
}]
