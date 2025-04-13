import express from "express";
import mongoose from 'mongoose';
import { MongoMemoryServer } from 'mongodb-memory-server';
import http from 'http'; // For proper server typing
import connectDB from "../db/connection";
import { login, register } from '../db/controllers/userController';

let mongoServer: MongoMemoryServer;
let server: http.Server;
let app: express.Express;

const createTestServer = async (): Promise<http.Server> => {
	mongoServer = await MongoMemoryServer.create();

	process.env.MONGO_CONNECTION_STRING = mongoServer.getUri();
	process.env.MONGO_INITDB_DATABASE = 'on-par-db';
	process.env.MONGO_USER = 'romulans';
	process.env.MONGO_PASS = 'snalumor';

	app = express();
	app.use(express.json());
	app.post('/api/users/register', register);
	app.post('/api/users/login', login);

	await connectDB();

	return new Promise((resolve) => {
		server = app.listen(0, () => {
			console.log(`Test server running on port ${server.address()}`);
			resolve(server);
		});
	});
};

beforeAll(async () => {
	server = await createTestServer();
});

afterAll(async () => {
	if (server) {
		await new Promise<void>((resolve, reject) => {
			server.close((err) => {
				if (err) {
					console.error('Error closing server:', err);
					reject(err);
				} else {
					console.log('Test server closed');
					resolve();
				}
			});
		});
	}

	if (mongoose.connection.readyState !== 0) {
		await mongoose.disconnect();
		console.log('MongoDB disconnected');
	}

	if (mongoServer) {
		await mongoServer.stop();
		console.log('MongoDB memory server stopped');
	}
});

export const getTestServer = (): http.Server => server;
export const getTestApp = (): express.Express => app;