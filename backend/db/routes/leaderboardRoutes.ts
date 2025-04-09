import { Router } from "express";
import { addScore, getTopUsers } from "../controllers/leaderboardController";

const leaderboardRouter = Router();

leaderboardRouter.post('/add', addScore);

leaderboardRouter.get('/top-users', getTopUsers);

export default leaderboardRouter;