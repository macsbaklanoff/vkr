import {Component, ElementRef, inject, Inject, ViewChild} from '@angular/core';
import Chart, {ChartTypeRegistry} from 'chart.js/auto';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import {EstimationService} from '../../../services/estimation.service';
import {IEstsAllGroups} from '../../../interfaces/ests-all-groups';

@Component({
  selector: 'app-graphics',
  imports: [],
  templateUrl: './graphics.component.html',
  styleUrl: './graphics.component.scss'
})
export class GraphicsComponent {
  @ViewChild('barCanvas') barCanvas: ElementRef | undefined;
  barChart: any;
  private readonly _estimationService = inject(EstimationService);
  private dataGraphics: number[] = []


  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    if (data == 'all') {
      console.log(data)
      this._estimationService.getEstimationsAllGroups().subscribe({
        next: (data): void => {
          this.dataGraphics = data
          this.barChartMethod();
          console.log(this.dataGraphics)
        }
      })
    }
    else {

    }
  }
  public test1 = this.dataGraphics[0]
  public test2 = this.dataGraphics[1];
  ngAfterViewInit(): void {

  }
  barChartMethod() {
    this.barChart = new Chart(this.barCanvas?.nativeElement, {
      type: `bar`,
      data: {
        labels: ['5', '4', '3', '2'],
        datasets: [
          {
            label: '5',
            data: this.dataGraphics,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              // 'rgba(153, 102, 255, 0.2)',
              // 'rgba(255, 159, 64, 0.2)',
            ],
            borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              // 'rgba(153, 102, 255, 1)',
              // 'rgba(255, 159, 64, 1)',
            ],
            borderWidth: 1,
          },
        ],
      },
      options: {
        plugins: {
          legend: {
            // display: true,
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
