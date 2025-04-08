import { createClient } from "redis";
import type { RedisClientType } from "redis";

let redisClient: RedisClientType | null = null;

const connectRedis = async () => {
    const redis_uri = process.env.REDIS_URL as string;
    const redis_port = parseInt(process.env.REDIS_PORT as string);

    try {
        redisClient = createClient({ 
            password: process.env.REDIS_PASSWORD,
            username: process.env.REDIS_USERNAME,
            socket: {
                host: redis_uri,
                port: redis_port,
            }
        });
        await redisClient.connect();
        console.log("Connected to Redis!")
        return redisClient;
    } catch (err) {
        console.error(`Failed to connect to Redis.\n${err}`);
        throw new Error(`Failed to connect to Redis.\n${err}`);
    }
}

export const getRedisClient = async () => {
    if (!redisClient) {
        await connectRedis();
    }
    return redisClient;
};

export default getRedisClient;