import { NextFunction, Response } from "express";
import { AuthenticatedRequest } from "../db/types";
import getRedisClient from "../db/redis";
import * as crypto from 'crypto';

export const checkTokenBlacklist = async (req: AuthenticatedRequest, res: Response, next: NextFunction) => {
    if(!req.headers['authorization']) {
        const err = new Error();
        err.name = 'UnauthorizedError';
        next(err);
    }
    if(await isTokenBlacklisted(req.headers['authorization']!.split(' ')[1] as string)) {
        const err = new Error();
        err.name = 'UnauthorizedError';
        next(err)
    } else
        next();
}

export const isTokenBlacklisted = async (token: string) => {
    const hashedToken = crypto.createHash('sha256').update(token).digest('hex'); // Hashed to reduce size
    const redisClient = await getRedisClient();
    console.log(await redisClient?.exists(`bl_${hashedToken}`) === 1);
    return await redisClient?.exists(`bl_${hashedToken}`) === 1;
}