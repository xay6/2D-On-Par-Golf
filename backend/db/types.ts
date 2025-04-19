import { Request } from 'express';

interface UserPayload {
  id: string;
  username: string;
}

export interface AuthenticatedRequest extends Request {
  user?: UserPayload;
  locals?: any
}