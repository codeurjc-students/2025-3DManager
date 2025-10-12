
import type { Response } from './Response';

export interface CommonResponse<T> extends Response {
    data?: T;
}