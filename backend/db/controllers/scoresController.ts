import { Request, Response } from "express";
import { CourseScores, IUserScores } from "../models/CourseScores"
import User from "../models/User";
import mongoose from "mongoose";

export const addOrUpdateScores = async (req: Request, res: Response): Promise<void> => {
    try {
        const { courseId, username, score } = req.body;
        if (!courseId || !username || typeof score !== 'number') {
            res.status(422).json({ message: 'Invalid input: courseId, username, and score are required.', success: false });
            return;
        }

        const user = await User.findOne({ username }).exec();
        if (!user) {
            res.status(404).json({ message: "User not found." });
            return;
        } else {
            const course = await CourseScores.findOne({ courseId });
            if (!course) {
                const newCourse = new CourseScores({
                    courseId,
                    userData: [{ user, score }]
                });
                await newCourse.save();
                res.status(200).json({ message: `New course created with ${username}'s score.`, success: true });
                console.log(`New course created with ${username}'s score.`);
                return;
            }

            const updateResult = await CourseScores.updateOne(
                { courseId, 'userData.user': user },
                { $set: { 'userData.$.score': score } }
            );

            res.status(200).json({ message: "Score changed successfully.", success: true });
        }

        // if (user) {
        //     // const updatedCourse = await CourseScores.findOneAndUpdate(
        //     //     { courseId },
        //     //     { $addToSet: { userData: { user: user._id, score } } },
        //     //     { upsert: true, new: true, setDefaultsOnInsert: true }
        //     // ).exec();
        //     await addScore(req, res); // Add score to Redis store.

        //     if(courseId as string === "overall") {
        //         return;
        //     }

        //     res.status(200).json({ message: "Score changed successfully.", success: true });
        // } else {
        //     res.status(404).json({ message: "User not found.", success: false });
        // }
    } catch (err: any) {
        res.status(500).json({ message: "An error occurred while changing the score.", success: false });
        console.error("addOrUpdateScore", err.message);
    }
}
