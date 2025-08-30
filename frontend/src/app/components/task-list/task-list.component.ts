import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { Task, TaskStatus, Priority } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule
  ],
  template: `
    <div class="task-list-container">
      <h2>Liste des Tâches</h2>
      
      <mat-table [dataSource]="tasks" class="task-table">
        <!-- Title Column -->
        <ng-container matColumnDef="title">
          <mat-header-cell *matHeaderCellDef>Titre</mat-header-cell>
          <mat-cell *matCellDef="let task">{{ task.title }}</mat-cell>
        </ng-container>

        <!-- Status Column -->
        <ng-container matColumnDef="status">
          <mat-header-cell *matHeaderCellDef>Statut</mat-header-cell>
          <mat-cell *matCellDef="let task">
            <mat-chip [class]="getStatusClass(task.status)">
              {{ getStatusLabel(task.status) }}
            </mat-chip>
          </mat-cell>
        </ng-container>

        <!-- Priority Column -->
        <ng-container matColumnDef="priority">
          <mat-header-cell *matHeaderCellDef>Priorité</mat-header-cell>
          <mat-cell *matCellDef="let task">
            <mat-chip [class]="getPriorityClass(task.priority)">
              {{ getPriorityLabel(task.priority) }}
            </mat-chip>
          </mat-cell>
        </ng-container>

        <!-- Due Date Column -->
        <ng-container matColumnDef="dueDate">
          <mat-header-cell *matHeaderCellDef>Échéance</mat-header-cell>
          <mat-cell *matCellDef="let task">
            {{ task.dueDate ? (task.dueDate | date:'dd/MM/yyyy') : 'Aucune' }}
          </mat-cell>
        </ng-container>

        <!-- Actions Column -->
        <ng-container matColumnDef="actions">
          <mat-header-cell *matHeaderCellDef>Actions</mat-header-cell>
          <mat-cell *matCellDef="let task">
            <button mat-icon-button (click)="viewTask(task.id)" matTooltip="Voir">
              <mat-icon>visibility</mat-icon>
            </button>
            <button mat-icon-button (click)="editTask(task.id)" matTooltip="Modifier">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button 
                    (click)="updateStatus(task)" 
                    [disabled]="task.status === TaskStatus.Completed"
                    matTooltip="Marquer comme terminé">
              <mat-icon>check_circle</mat-icon>
            </button>
            <button mat-icon-button (click)="deleteTask(task)" matTooltip="Supprimer">
              <mat-icon>delete</mat-icon>
            </button>
          </mat-cell>
        </ng-container>

        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
      </mat-table>

      <div *ngIf="tasks.length === 0" class="no-tasks">
        <p>Aucune tâche trouvée</p>
        <button mat-raised-button color="primary" routerLink="/tasks/new">
          Créer votre première tâche
        </button>
      </div>
    </div>
  `,
  styles: [`
    .task-list-container {
      padding: 20px;
    }

    .task-table {
      width: 100%;
      margin-top: 20px;
    }

    .no-tasks {
      text-align: center;
      padding: 40px;
    }

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
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  displayedColumns: string[] = ['title', 'status', 'priority', 'dueDate', 'actions'];
  TaskStatus = TaskStatus;

  constructor(
    private taskService: TaskService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getAllTasks().subscribe({
      next: (tasks) => this.tasks = tasks,
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.snackBar.open('Erreur lors du chargement des tâches', 'Fermer', { duration: 3000 });
      }
    });
  }

  viewTask(id: string): void {
    this.router.navigate(['/tasks', id]);
  }

  editTask(id: string): void {
    this.router.navigate(['/tasks', id, 'edit']);
  }

  updateStatus(task: Task): void {
    if (task.status === TaskStatus.Completed) return;

    const newStatus = task.status === TaskStatus.Pending ? TaskStatus.InProgress : TaskStatus.Completed;
    
    this.taskService.updateTaskStatus(task.id, { status: newStatus }).subscribe({
      next: () => {
        this.loadTasks();
        this.snackBar.open('Statut mis à jour', 'Fermer', { duration: 2000 });
      },
      error: (error) => {
        console.error('Error updating task status:', error);
        this.snackBar.open('Erreur lors de la mise à jour', 'Fermer', { duration: 3000 });
      }
    });
  }

  deleteTask(task: Task): void {
    if (confirm(`Êtes-vous sûr de vouloir supprimer la tâche "${task.title}" ?`)) {
      this.taskService.deleteTask(task.id).subscribe({
        next: () => {
          this.loadTasks();
          this.snackBar.open('Tâche supprimée', 'Fermer', { duration: 2000 });
        },
        error: (error) => {
          console.error('Error deleting task:', error);
          this.snackBar.open('Erreur lors de la suppression', 'Fermer', { duration: 3000 });
        }
      });
    }
  }

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
}
