import { Router } from "express";
import { addScore } from "../controllers/leaderboardController";

const leaderboardRouter = Router();

leaderboardRouter.post('/add', addScore);

export default leaderboardRouter;