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
        console.error('Error in updateCoins:\n', err);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
        return;
    }
}]

export const updateRewards = [authenticateJwt, async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { username, reward } = req.body;
        if (!username && !reward) {
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
            const rewards = [reward]
            const newUserItems = new UserItems({
                user,
                rewards: rewards || [],
            });
            await newUserItems.save();
            res.status(200).json({ message: `Items created for ${username}.`, success: true });
            console.log(`Items created for ${username}.`);
            return;
        }

        if (userItems) {
            userItems.rewards.push(reward);
            await userItems.save()
        }

        res.status(200).json({ message: `Items changed for ${username}.`, success: true });
        console.log(`Items changed for ${username}.`);

    } catch (err: any) {
        console.error('Error in updateRewards:\n', err);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
        return;
    }
}]

export const getCoins = [authenticateJwt, async (req: AuthenticatedRequest, res: Response) => {
    try {
        const { username } = req.body;
        if(!username) {
            res.status(422).json({ message: "Invalid input: Username is required.", success: false });
            return;
        }

        const user = await User.findOne({ username });
        if (!user) {
            res.status(404).json({ message: "User not found.", success: false });
            console.error("User not found.");
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to fetch this users coins.", success: false });
            console.error("You are not authorized to fetch this users coins.");
            return;
        }

        const userItems = await UserItems.findOne({ user });
        if (!userItems) {
            const newUserItems = new UserItems({
                user,
                coinAmount: 0,
            });
            await newUserItems.save();
            res.status(200).json({ message: `Coins created for ${username}.`, success: true });
            console.log(`Coins created for ${username}.`);
            return;
        }

        res.status(200).json({ message: `${username} has ${userItems.coinAmount} coins.`, coinAmount: userItems.coinAmount, success: true });
        console.log(`Coins fetched for ${username}.`);
    } catch (err: any) {
        console.error('Error in updateRewards:\n', err);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
        return;
    }
}]

export const getRewards = [authenticateJwt, async (req: AuthenticatedRequest, res: Response) => {
    try {
        const { username } = req.body;
        if(!username) {
            res.status(422).json({ message: "Invalid input: Username is required.", success: false });
            return;
        }

        const user = await User.findOne({ username });
        if (!user) {
            res.status(404).json({ message: "User not found.", success: false });
            console.error("User not found.");
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to fetch the users items.", success: false });
            console.error("You are not authorized to fetch this users items.");
            return;
        }

        const userItems = await UserItems.findOne({ user });
        if (!userItems) {
            const newUserItems = new UserItems({
                user,
                rewards: []
            });
            await newUserItems.save();
            res.status(200).json({ message: `Rewards created for ${username}.`, success: true });
            console.log(`Rewards created for ${username}.`);
            return;
        }

        res.status(200).json({ message: `Items fetched for ${username}.`, rewards: userItems.rewards, success: true });
        console.log(`Items fetched for ${username}.`);
    } catch (err: any) {
        console.error('Error in updateRewards:\n', err);
        res.status(500).json({ message: `Internal server error: ${err.message}`, success: false });
        return;
    }
}]