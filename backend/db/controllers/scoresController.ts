import { Request, Response } from "express";
import { CourseScores, IUserScores } from "../models/CourseScores"
import User from "../models/User";
import mongoose from "mongoose";
export const addUpdateScores = async (req: Request, res: Response): Promise<void> => {
    try {
        const { courseId, username, score } = req.body;
        if (!courseId || !username || typeof score !== 'number') {
            res.status(422).json({ message: 'Invalid input: courseId, username, and score are required.', success: false });
            console.log('Invalid input: courseId, username, and score are required.');
            return;
        }

        const user = await User.findOne({ username });
        if (!user) {
            res.status(404).json({ message: "User not found." });
            console.log("User not found.");
            return;
        } else {
            const course = await CourseScores.findOne({ courseId });
            if (!course) {
                const newCourse = new CourseScores({
                    courseId,
                    userData: [{ user, score }]
                });
                await newCourse.save();
                await addScore(req, res); // Add score to Redis store.
                res.status(200).json({ message: `New course created with ${username}'s score.`, success: true });
                console.log(`New course created with ${username}'s score.`);
                return;
            }

            await addScore(req, res); // Add score to Redis store.
            // Finish execution if overall score is being updated. No need to store this in MongoDB
            if (courseId as string === "overall") {
                res.status(200).json({ message: "Overall score changed successfully.", success: true });
                console.log("Overall score changed successfully.");
                return;
            }

            await CourseScores.updateOne(
                { courseId },
                { $addToSet: { userData: { user: user._id, score } } },
                { upsert: true }
            );

            res.status(200).json({ message: "Score changed successfully.", success: true });
            console.log("Score changed successfully.");
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
        console.error("Error in addOrUpdateScore", err.message);
    }
}
