import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TaskService } from '../../services/task.service';
import { Priority } from '../../models/task.model';
import { CommonModule } from '@angular/common';

// Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  template: `
    <div class="task-form-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ isEditMode ? 'Modifier la tâche' : 'Nouvelle tâche' }}</mat-card-title>
        </mat-card-header>
        
        <mat-card-content>
          <form [formGroup]="taskForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Titre</mat-label>
              <input matInput formControlName="title" required>
              <mat-error *ngIf="taskForm.get('title')?.hasError('required')">
                Le titre est requis
              </mat-error>
              <mat-error *ngIf="taskForm.get('title')?.hasError('maxlength')">
                Le titre ne peut pas dépasser 200 caractères
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Description</mat-label>
              <textarea matInput formControlName="description" rows="4"></textarea>
              <mat-error *ngIf="taskForm.get('description')?.hasError('maxlength')">
                La description ne peut pas dépasser 1000 caractères
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Priorité</mat-label>
              <mat-select formControlName="priority" required>
                <mat-option [value]="Priority.Low">Faible</mat-option>
                <mat-option [value]="Priority.Medium">Moyenne</mat-option>
                <mat-option [value]="Priority.High">Élevée</mat-option>
                <mat-option [value]="Priority.Critical">Critique</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Date d'échéance</mat-label>
              <input matInput [matDatepicker]="picker" formControlName="dueDate">
              <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
              <mat-datepicker #picker></mat-datepicker>
            </mat-form-field>
          </form>
        </mat-card-content>

        <mat-card-actions align="end">
          <button mat-button type="button" (click)="onCancel()">Annuler</button>
          <button mat-raised-button 
                  color="primary" 
                  (click)="onSubmit()" 
                  [disabled]="taskForm.invalid || isLoading">
            {{ isEditMode ? 'Mettre à jour' : 'Créer' }}
          </button>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .task-form-container {
      max-width: 600px;
      margin: 0 auto;
      padding: 20px;
    }

    .full-width {
      width: 100%;
      margin-bottom: 15px;
    }

    mat-card-actions {
      padding: 16px;
    }
  `]
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  isEditMode = false;
  isLoading = false;
  taskId?: string;
  Priority = Priority;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(1000)]],
      priority: [Priority.Medium, Validators.required],
      dueDate: ['']
    });
  }

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.taskId && this.route.snapshot.url.some(segment => segment.path === 'edit');

    if (this.isEditMode && this.taskId) {
      this.loadTask(this.taskId);
    }
  }

  loadTask(id: string): void {
    this.taskService.getTask(id).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          priority: task.priority,
          dueDate: task.dueDate ? new Date(task.dueDate) : null
        });
      },
      error: (error) => {
        console.error('Error loading task:', error);
        this.snackBar.open('Erreur lors du chargement de la tâche', 'Fermer', { duration: 3000 });
        this.router.navigate(['/tasks']);
      }
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.isLoading = true;
      const formValue = this.taskForm.value;
      
      const taskData = {
        title: formValue.title,
        description: formValue.description || undefined,
        priority: formValue.priority,
        dueDate: formValue.dueDate ? formValue.dueDate.toISOString() : undefined
      };

      const operation = this.isEditMode && this.taskId
        ? this.taskService.updateTask(this.taskId, taskData)
        : this.taskService.createTask(taskData);

      operation.subscribe({
        next: () => {
          this.isLoading = false;
          const message = this.isEditMode ? 'Tâche mise à jour' : 'Tâche créée';
          this.snackBar.open(message, 'Fermer', { duration: 2000 });
          this.router.navigate(['/tasks']);
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Error saving task:', error);
          this.snackBar.open('Erreur lors de la sauvegarde', 'Fermer', { duration: 3000 });
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/tasks']);
  }
}
