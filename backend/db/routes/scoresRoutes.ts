import express from "express";
import { getScore, addUpdateScores, deleteScore } from "../controllers/scoresController";

const scoresRouter = express.Router();

scoresRouter.get("/get-score", getScore);

// scoresRouter.post("/scores/create", createScore);

scoresRouter.put("/add-update", addUpdateScores);

scoresRouter.delete("/delete-score", deleteScore);

export default scoresRouter;