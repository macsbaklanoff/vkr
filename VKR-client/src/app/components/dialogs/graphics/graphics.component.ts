import {Component, ElementRef, inject, Inject, ViewChild} from '@angular/core';
import Chart, {ChartTypeRegistry} from 'chart.js/auto';
import {MAT_DIALOG_DATA, MatDialogClose} from '@angular/material/dialog';
import {EstimationService} from '../../../services/estimation.service';
import {IGroupResponse} from '../../../interfaces/group-response';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-graphics',
  imports: [
    MatButton,
    MatDialogClose
  ],
  templateUrl: './graphics.component.html',
  styleUrl: './graphics.component.scss'
})
export class GraphicsComponent {
  @ViewChild('barCanvas') barCanvas: ElementRef | undefined;
  barChart: any;
  private readonly _estimationService = inject(EstimationService);
  private dataGraphics: number[] = []
  public graphicName : string = "";

  constructor(@Inject(MAT_DIALOG_DATA) public data: {group: string, favoriteGroup: IGroupResponse}) {
    if (data.group == 'all') {
      this.graphicName = 'Оценки работ всем группам'
      this._estimationService.getEstimationsAllGroups().subscribe({
        next: (data): void => {
          this.dataGraphics = data
          this.barChartMethod();
          console.log(this.dataGraphics)
        }
      })
    }
    else if (data.group == 'one') {
      this.graphicName = `Оценки работ по группе ${data.favoriteGroup.groupName}`
      this._estimationService.getEstimationsGroup(data.favoriteGroup.groupId).subscribe({
        next: (data): void => {
          this.dataGraphics = data
          this.barChartMethod();
          console.log(this.dataGraphics)
        }
      })
    }
  }

  barChartMethod() {
    this.barChart = new Chart(this.barCanvas?.nativeElement, {
      type: `bar`,
      data: {
        labels: ['5', '4', '3', '2'],
        datasets: [
          {
            label: '',
            data: this.dataGraphics,
            backgroundColor: [
              'rgba(69, 168, 91, 0,5)',
              'rgba(219, 215, 101, 0.5)',
              'rgba(235, 161, 52, 0.5)',
              'rgba(219, 66, 66, 0.5)',
              // 'rgba(153, 102, 255, 0.2)',
              // 'rgba(255, 159, 64, 0.2)',
            ],
            borderColor: [
              'rgba(69, 168, 91, 1)',
              'rgba(219, 215, 101, 1)',
              'rgba(235, 161, 52, 1)',
              'rgba(219, 66, 66, 1)',
              // 'rgba(153, 102, 255, 1)',
              // 'rgba(255, 159, 64, 1)',
            ],
            borderWidth: 2,
          },
        ],
      },
      options: {
        plugins: {
          legend: {
            display: false,
            position: 'bottom',
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Количество'
            }
          },
          x: {
            title: {
              display: true,
              text: 'Оценки'
            }
          }
        }
      },
    });
  }
}
