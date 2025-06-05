export interface IRegisterRequest {
  email: string;
  password: string;
  firstName: string;
  patronymic: string | null;
  lastName: string;
  groupName: string | undefined;
}
