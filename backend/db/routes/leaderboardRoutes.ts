import { Router } from "express";
import { addScore, getTopUsers, getUserRank } from "../controllers/leaderboardController";

const leaderboardRouter = Router();

leaderboardRouter.post('/add', addScore);

leaderboardRouter.get('/top-users', getTopUsers);

leaderboardRouter.get('/get-rank', getUserRank);

export default leaderboardRouter;