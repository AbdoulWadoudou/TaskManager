export enum TaskStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3
}

export enum Priority {
  Low = 0,
  Medium = 1,
  High = 2,
  Critical = 3
}

export interface Task {
  id: string;
  title: string;
  description?: string;
  status: TaskStatus;
  priority: Priority;
  dueDate?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  priority: Priority;
  dueDate?: string;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string;
  priority: Priority;
  dueDate?: string;
}

export interface UpdateTaskStatusRequest {
  status: TaskStatus;
}