import { expressjwt } from 'express-jwt';
import dotenv from 'dotenv'
dotenv.config();

export const authenticateJwt = expressjwt({
    secret: process.env.JWT_SECRET as string,
    algorithms: ['HS256'],
    requestProperty: 'user'
});