import { Request, Response } from "express";
import CourseScores, { IUserScores } from "../models/CourseScores"
import User from "../models/User";
import mongoose from "mongoose";

export const getScore = async (req: Request, res: Response): Promise<void> => {
    try {
        const { courseId, username } = req.body;

        if (!courseId || !username) {
            res.status(400).json({ message: 'Invalid input: courseId and username are required.', success: false });
            return;
        }

        const user = await User.findOne({ username }).exec();
        const scores = await CourseScores.findOne({ courseId }).populate('userData.user').exec();

        if (scores && user) {
            const userScore: IUserScores | undefined = scores.userData.find(score => {
                if (score.user) return score.user._id.equals(user._id as mongoose.mongo.ObjectId);
            });

            if (userScore) {
                res.status(200).json({ message: "Score fetched successfully.", userScore: { courseId, username, score: userScore.score }, success: true });
                console.log({ message: "Score fetched successfully." });
            } else {
                res.status(404).json({ message: "User score not found.", success: false });
                console.error("User score not found.");
            }
        }
    } catch (err: any) {
        res.status(500).json({ message: "An error occurred while adding the score.", success: false });
        console.error("Error in getScore", err.message);
    }
}

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
                res.status(200).json({ message: `New course created with ${username}'s score.`, success: true });
                console.log(`New course created with ${username}'s score.`);
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

// export const createScore = async (req: Request, res: Response): Promise<void> => {
//     try {
//         const { courseId, username, score } = req.body;
//         const user = await User.findOne({ username }).exec();

//         const updatedCourse = await CourseScores.findOneAndUpdate(
//             { courseId },
//             { $addToSet: { userData: { user, score } } },
//             { upsert: true, new: true, setDefaultsOnInsert: true }
//         ).exec();

//         res.status(200).json({ message: "Score created successfully.", updatedCourse });
//     } catch (err: any) {
//         res.status(500).json({ message: "An error occurred while adding the score." });
//         console.error(err);
//     }
// }