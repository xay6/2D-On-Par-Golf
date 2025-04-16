import express, { NextFunction, Request, Response } from 'express';

import connectDB from "./db/connection";
import userRoutes from './db/routes/userRoutes';
import scoresRoutes from './db/routes/scoresRoutes';

import leaderboardRoutes from './db/routes/leaderboardRoutes';

import * as dotenv from "dotenv";
import userItemsRoutes from './db/routes/userItemsRoutes';
dotenv.config();
    
const port = process.env.PORT;

const app = express();
app.use(express.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Server running.');
});

app.use("/api/users", userRoutes);
app.use("/api/scores", scoresRoutes);
app.use("/api/leaderboard", leaderboardRoutes);
app.use("/api/items", userItemsRoutes);

app.use((err: any, req: Request, res: Response, next: NextFunction) => {
  if (err.name === 'UnauthorizedError') {
      // Handle express-jwt errors
      console.log("User not authorized to perform this action. Invalid or missing token.");
      res.status(401).json({ message: 'User not authorized to perform this action. Invalid or missing token.', success: false });
  }
});

connectDB();

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
