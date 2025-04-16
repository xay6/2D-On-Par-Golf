import express from "express";
import { getScore, addUpdateScores, deleteScore } from "../controllers/scoresController";
import { addScore } from "../controllers/leaderboardController";

const scoresRouter = express.Router();

scoresRouter.get("/get-score", getScore);

// scoresRouter.post("/scores/create", createScore);

scoresRouter.put("/add-update", addUpdateScores, addScore);

scoresRouter.delete("/delete-score", deleteScore);

export default scoresRouter;