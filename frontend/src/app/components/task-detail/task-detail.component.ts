import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Task, TaskStatus, Priority } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { CommonModule, DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatChipsModule,
    MatSnackBarModule,
    MatButtonModule
  ],
  template: `
    <div class="task-detail-container" *ngIf="task">
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ task.title }}</mat-card-title>
          <mat-card-subtitle>
            Créée le {{ task.createdAt | date:'dd/MM/yyyy à HH:mm' }}
            <span *ngIf="task.updatedAt"> - Modifiée le {{ task.updatedAt | date:'dd/MM/yyyy à HH:mm' }}</span>
          </mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <div class="task-info">
            <div class="info-row">
              <strong>Statut:</strong>
              <mat-chip [class]="getStatusClass(task.status)">
                {{ getStatusLabel(task.status) }}
              </mat-chip>
            </div>

            <div class="info-row">
              <strong>Priorité:</strong>
              <mat-chip [class]="getPriorityClass(task.priority)">
                {{ getPriorityLabel(task.priority) }}
              </mat-chip>
            </div>

            <div class="info-row" *ngIf="task.dueDate">
              <strong>Date d'échéance:</strong>
              <span>{{ task.dueDate | date:'dd/MM/yyyy' }}</span>
            </div>

            <div class="info-row" *ngIf="task.description">
              <strong>Description:</strong>
              <p>{{ task.description }}</p>
            </div>
          </div>
        </mat-card-content>

        <mat-card-actions align="end">
          <button mat-button (click)="goBack()">Retour</button>
          <button mat-raised-button color="primary" (click)="editTask()">Modifier</button>
          <button mat-raised-button 
                  color="accent" 
                  (click)="updateStatus()"
                  [disabled]="task.status === TaskStatus.Completed">
            {{ getNextStatusLabel() }}
          </button>
        </mat-card-actions>
      </mat-card>
    </div>

    <div *ngIf="!task" class="loading">
      Chargement...
    </div>
  `,
  styles: [`
    .task-detail-container { max-width: 800px; margin: 0 auto; padding: 20px; }
    .task-info { margin-top: 20px; }
    .info-row { margin-bottom: 15px; display: flex; align-items: center; gap: 10px; }
    .info-row p { margin: 0; margin-top: 5px; white-space: pre-wrap; }
    .loading { text-align: center; padding: 40px; }
    .status-pending { background-color: #ffc107; }
    .status-in-progress { background-color: #2196f3; }
    .status-completed { background-color: #4caf50; }
    .status-cancelled { background-color: #f44336; }
    .priority-low { background-color: #e8f5e8; }
    .priority-medium { background-color: #fff3e0; }
    .priority-high { background-color: #ffebee; }
    .priority-critical { background-color: #f44336; color: white; }
  `]
})
export class TaskDetailComponent implements OnInit {
  task?: Task;
  TaskStatus = TaskStatus;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadTask(id);
  }

  loadTask(id: string): void {
    this.taskService.getTask(id).subscribe({
      next: task => this.task = task,
      error: error => {
        console.error('Error loading task:', error);
        this.snackBar.open('Erreur lors du chargement de la tâche', 'Fermer', { duration: 3000 });
        this.router.navigate(['/tasks']);
      }
    });
  }

  editTask(): void { if (this.task) this.router.navigate(['/tasks', this.task.id, 'edit']); }

  updateStatus(): void {
    if (!this.task || this.task.status === TaskStatus.Completed) return;
    const newStatus = this.task.status === TaskStatus.Pending ? TaskStatus.InProgress : TaskStatus.Completed;
    this.taskService.updateTaskStatus(this.task.id, { status: newStatus }).subscribe({
      next: updatedTask => {
        this.task = updatedTask;
        this.snackBar.open('Statut mis à jour', 'Fermer', { duration: 2000 });
      },
      error: error => {
        console.error('Error updating task status:', error);
        this.snackBar.open('Erreur lors de la mise à jour', 'Fermer', { duration: 3000 });
      }
    });
  }

  goBack(): void { this.router.navigate(['/tasks']); }

  getStatusLabel(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.Pending: return 'En attente';
      case TaskStatus.InProgress: return 'En cours';
      case TaskStatus.Completed: return 'Terminé';
      case TaskStatus.Cancelled: return 'Annulé';
      default: return 'Inconnu';
    }
  }

  getStatusClass(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.Pending: return 'status-pending';
      case TaskStatus.InProgress: return 'status-in-progress';
      case TaskStatus.Completed: return 'status-completed';
      case TaskStatus.Cancelled: return 'status-cancelled';
      default: return '';
    }
  }

  getPriorityLabel(priority: Priority): string {
    switch (priority) {
      case Priority.Low: return 'Faible';
      case Priority.Medium: return 'Moyenne';
      case Priority.High: return 'Élevée';
      case Priority.Critical: return 'Critique';
      default: return 'Inconnue';
    }
  }

  getPriorityClass(priority: Priority): string {
    switch (priority) {
      case Priority.Low: return 'priority-low';
      case Priority.Medium: return 'priority-medium';
      case Priority.High: return 'priority-high';
      case Priority.Critical: return 'priority-critical';
      default: return '';
    }
  }

  getNextStatusLabel(): string {
    if (!this.task) return '';
    switch (this.task.status) {
      case TaskStatus.Pending: return 'Commencer';
      case TaskStatus.InProgress: return 'Terminer';
      case TaskStatus.Completed: return 'Terminé';
      default: return 'Mettre à jour';
    }
  }
}
