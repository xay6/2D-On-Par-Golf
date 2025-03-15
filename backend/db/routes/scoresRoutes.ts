import express from "express";
import { addOrUpdateScores } from "../controllers/scoresController";

const scoresRouter = express.Router();

scoresRouter.put("/add-update", addOrUpdateScores);

export default scoresRouter;