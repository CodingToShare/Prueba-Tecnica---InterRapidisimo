import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

export interface ConfirmDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  type?: 'warning' | 'danger' | 'info';
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="confirm-dialog">
      <div class="dialog-header" [ngClass]="'header-' + data.type">
        <mat-icon class="dialog-icon">
          @if (data.type === 'danger') {
            warning
          } @else if (data.type === 'warning') {
            error_outline
          } @else {
            info
          }
        </mat-icon>
        <h2 mat-dialog-title>{{ data.title }}</h2>
      </div>

      <mat-dialog-content>
        <p class="dialog-message">{{ data.message }}</p>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="onCancel()">
          {{ data.cancelText || 'Cancelar' }}
        </button>
        <button
          mat-raised-button
          [color]="data.type === 'danger' ? 'warn' : 'primary'"
          (click)="onConfirm()">
          {{ data.confirmText || 'Confirmar' }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .confirm-dialog {
      min-width: 400px;
      max-width: 500px;
    }

    .dialog-header {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 24px 24px 16px;
      margin: -24px -24px 0;
      border-radius: 12px 12px 0 0;

      &.header-danger {
        background: linear-gradient(135deg, #fee2e2, #fecaca);

        .dialog-icon {
          color: #dc2626;
        }

        h2 {
          color: #991b1b;
        }
      }

      &.header-warning {
        background: linear-gradient(135deg, #fef3c7, #fde68a);

        .dialog-icon {
          color: #d97706;
        }

        h2 {
          color: #92400e;
        }
      }

      &.header-info {
        background: linear-gradient(135deg, #dbeafe, #bfdbfe);

        .dialog-icon {
          color: #2563eb;
        }

        h2 {
          color: #1e40af;
        }
      }
    }

    .dialog-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
    }

    h2 {
      margin: 0;
      font-size: 20px;
      font-weight: 600;
    }

    mat-dialog-content {
      padding: 24px !important;
      margin: 0 !important;
    }

    .dialog-message {
      font-size: 15px;
      line-height: 1.6;
      color: #475569;
      margin: 0;
    }

    mat-dialog-actions {
      padding: 16px 24px 24px !important;
      margin: 0 !important;
      gap: 12px !important;
    }
  `]
})
export class ConfirmDialogComponent {
  data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
