import { Request, Response } from "express";
import CourseScores, { IUserScores } from "../models/CourseScores"
import User from "../models/User";
import mongoose, { ObjectId, Types } from "mongoose";
import { AuthenticatedRequest } from "../types";
import { authenticateJwt } from "../../middleware/authenticateJwt";

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

export const addUpdateScores = [authenticateJwt, async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { courseId, username } = req.body;
        const score = parseInt(req.body.score, 10);
        let message = "";

        if (!courseId || !username || isNaN(score) || score < 1) {
            res.status(422).json({ message: 'Invalid input: courseId, username, and score are required.', success: false });
            console.error('Invalid input: courseId, username, and score are required.');
            return;
        }

        const user = await User.findOne({ username });

        if (!user) {
            res.status(404).json({ message: "User not found.", success: false });
            console.error("User not found.");
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to set this score.", success: false });
            console.error("You are not authorized to set this score.");
            return;
        }

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

        const courseWithUser = await CourseScores.findOne({
            courseId: courseId,
            "userData.user": user,
        });

        if (courseWithUser) {
            const result = await CourseScores.updateOne(
                {
                    courseId: courseId,
                    "userData": {
                        $elemMatch: {
                            user,
                            score: { $gt: score },
                        }
                    }
                },
                {
                    $set: { "userData.$.score": score },
                }
            );

            if (result.modifiedCount > 0) {
                message = "Score updated successfully.";
            } else {
                message = "Score was already up to date.";
            }
        } else {
            const result = await CourseScores.updateOne(
                {
                    courseId: courseId,
                },
                {
                    $push: { userData: { user: user, score: score } },
                },
                { upsert: true }
            );

            if (result.upsertedCount > 0) {
                message = "New course and user entry created.";
            } else if (result.modifiedCount > 0) {
                message = "New user entry added to existing course.";
            } else {
                message = "No changes made.";
            }
        }

        res.status(200).json({ message, success: true });
        console.log(message);
    } catch (err: any) {
        res.status(500).json({ message: "An error occurred while changing the score.", success: false });
        console.error("Error in addOrUpdateScore", err);
    }
}]

export const deleteScore = [authenticateJwt, async (req: AuthenticatedRequest, res: Response) => {
    try {
        const { username, courseId } = req.body;

        if (!username || !courseId) {
            res.status(422).json({ message: "Username and courseId required.", success: false });
            return;
        }

        const user = await User.findOne({ username }).exec();

        if (!user) {
            res.status(404).json({ message: "User not found", success: false });
            return;
        }

        if (req.user?.username !== username && req.user?.id !== user._id) {
            res.status(403).json({ message: "You are not authorized to delete this score.", success: false });
            return;
        }

        const course = await CourseScores.findOne({ courseId }).exec();

        if (!course) {
            res.status(404).json({ message: `Course '${courseId}' not found.`, success: false });
            return;
        }

        if (!await course.userData.find((user1: IUserScores) =>
            (user1.user as Types.ObjectId).toString() === (user._id as ObjectId).toString())) {
            res.status(404).json({ message: "Score not found.", success: false });
            console.log("Score not found.");
            return;
        }

        await CourseScores.updateOne(
            { courseId },
            { $pull: { userData: { user: user._id } } }
        );

        res.status(200).json({ message: "Score deleted.", success: true });
    } catch (err: any) {
        res.status(500).json({ message: "An error occurred when deleting the score.", error: err.message, success: false });
        console.log("Error in deleteScore: ", err);
    }
}]

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