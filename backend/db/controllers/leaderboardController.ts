import { Response } from "express";
import getRedisClient from "../redis";
import { AuthenticatedRequest } from "../types";

export const addScore = async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { courseId, username } = req.body;
        const score = parseInt(req.body.score, 10);
        if (!courseId || !username || isNaN(score)) {
            let message = `${req.locals.message}\nRedis: Invalid input: courseId, username, and score are required.`;
            console.log('Redis: Invalid input: courseId, username, and score are required.');
            res.status(400).json({ message, success: (req.locals.success) });
            return;
        }

        const redisClient = await getRedisClient();
        await redisClient?.zAdd(courseId, { score, value: username });

        if(req.locals.globalScore > 0)
            await redisClient?.zAdd("global", { score: req.locals.globalScore, value: username })

        let leaderboard = await redisClient?.zRangeWithScores(courseId, 0, -1);
        
        let message = `${req.locals.message}\nScore added successfully`;
        res.status(200).json({ message: message, success: req.locals.success });
        console.log('Score added successfully');
    } catch (err: any) {
        res.status(500).json({ message: `Error in addScore:\n ${err}`, success: req.locals.success });
        console.error('Error in addScore:\n', err);
        return;
    }
}

export const getTopUsers = async (req: AuthenticatedRequest, res: Response): Promise<void> => {
    try {
        const { courseId } = req.body;

        if (!courseId) {
            res.status(400).json({ message: 'Invalid input: courseId is required.', success: false });
            return;
        }

        if(!req.query.lowerlimit && !req.query.upperLimit) {
            res.status(400).json({ message: 'Invalid input: upper and lower limits invalid.', success: false });
            return;
        }
        
        const lowerLimit = parseInt(req.query.lowerlimit as string) || 0;
        const upperLimit = parseInt(req.query.upperlimit as string) || 10;
        
        const redisClient = await getRedisClient();
        const topUsers = await redisClient?.zRangeWithScores(courseId, lowerLimit, upperLimit);
        
        res.status(200).json({ data: topUsers, success: true });
    } catch (err: any) {
        console.error('Error in getTopUsers:\n', err);
        res.status(500).json({ message: 'Internal server error', success: false });
        return;
    }
}