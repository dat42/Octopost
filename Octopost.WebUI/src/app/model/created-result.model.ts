import { Request } from './request.model';

export class CreatedResult extends Request {
    public entity: string | undefined = undefined;
    public createdId: number | undefined = undefined;
}
