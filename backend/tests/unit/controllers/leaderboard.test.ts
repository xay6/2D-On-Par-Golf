import request from 'supertest';
import { getTestApp } from '../../setup';
import getRedisClient from '../../../db/redis';

jest.mock('redis', () => ({
    createClient: jest.fn(() => ({
        connect: jest.fn(),
        on: jest.fn(),
        get: jest.fn(),
        set: jest.fn(),
        del: jest.fn(),
        disconnect: jest.fn(),
        isReady: true
    })),
    zAdd: jest.fn()
}));

describe('Leaderboard Fetching - Success Case', () => {
    let redisClient: any;
    let server: any;

    beforeAll(async () => {
        redisClient = await getRedisClient();
        server = getTestApp()
    });

    it('should return the leaderboards top users', async () => {
        await redisClient.zAdd('leaderboard', [
            { score: 998, value: 'user1' },
        ]);
        await redisClient.zAdd('test', [
            { score: 999, value: 'user2' }
        ]);

        const response = await request(server)
            .get('/api/leaderboard/top-users')
            .send({
                courseId: 'test',
            })
            .expect(200);

        expect(response.body).toEqual({
            data: [
                { score: 998, value: 'user1' },
                { score: 999, value: 'user2' },
            ],
            success: true
        });
    });
});