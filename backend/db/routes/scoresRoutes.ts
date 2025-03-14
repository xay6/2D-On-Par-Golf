import express from "express";
import { getScore, addUpdateScores } from "../controllers/scoresController";

const scoresRouter = express.Router();

scoresRouter.get("/get-score", getScore);

// scoresRouter.post("/scores/create", createScore);

scoresRouter.put("/add-update", addUpdateScores);

export default scoresRouter;