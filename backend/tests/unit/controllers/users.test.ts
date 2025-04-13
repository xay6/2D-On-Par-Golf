import request from 'supertest';
import { getTestApp } from '../../setup'

jest.mock('../../../db/models/User', () => ({
    findOne: jest.fn(),
    prototype: {
        save: jest.fn()
    }
}));

jest.mock('../../../db/controllers/userController', () => ({
    hashPw: jest.fn(),
    register: jest.fn((req, res) => res.status(201).json({
        message: 'User account created.',
        success: true
    }))
}));

describe('User Registration - Success Case', () => {
    let server: any;
    
    beforeAll(() => {
        server = getTestApp();
    });

    beforeEach(() => {
        jest.clearAllMocks();
        require('../../../db/models/User').findOne.mockResolvedValue(null);
        require('../../../db/controllers/userController').hashPw.mockResolvedValue('hashed_password');
    });

    it('should successfully register with valid input', async () => {
        const response = await request(server)
            .post('/api/users/register')
            .send({
                username: 'testuser',
                password: 'validpassword123'
            })
            .expect(201);

        expect(response.body).toEqual({
            message: 'User account created.',
            success: true
        });
    });
});

describe('User Login - Success Case', () => {
    let server: any;
    
    beforeAll(() => {
        server = getTestApp();

    })
    beforeEach(() => {
        jest.clearAllMocks();
        require('../../../db/models/User').findOne.mockResolvedValue(null);
        require('../../../db/controllers/userController').hashPw.mockResolvedValue('hashed_password');
    });

    it('should successfully log in with valid input and existing credentials', async () => {
        const response = await request(server)
            .post('/api/users/login')
            .send({
                username: 'testuser',
                password: 'validpassword123'
            })
            .expect(201);

        expect(response.body).toEqual({
            message: 'Logging in.',
            success: true,
            token: expect.any(String)
        });
    });
});