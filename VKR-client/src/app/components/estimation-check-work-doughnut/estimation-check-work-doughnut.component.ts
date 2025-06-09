import {Component, ElementRef, Inject, inject, Input, OnInit, ViewChild} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MAT_DIALOG_DATA, MatDialogClose} from "@angular/material/dialog";
import {EstimationService} from '../../services/estimation.service';
import {IGroupResponse} from '../../interfaces/group-response';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-estimation-check-work-doughnut',
  imports: [
    MatButton,
    MatDialogClose
  ],
  templateUrl: './estimation-check-work-doughnut.component.html',
  styleUrl: './estimation-check-work-doughnut.component.scss'
})
export class EstimationCheckWorkDoughnutComponent {
  @Input() name: string = '';
  @Input() result: number = 0;
  @Input() estimationFrom: number = 0;
  @ViewChild('doughnutCanvas') doughnutCanvas: ElementRef | undefined;
  private remain: number = 0;
  doughnutChart: any;
  constructor() {
  }

  ngAfterViewInit() {
    this.remain = this.estimationFrom - this.result;
    // console.log(this.result);
    this.doughnutChartMethod();
  }

  public getColor(estimation: number) : [string, string] {
    console.log(estimation);
    if (estimation >= 0.81) return ['rgba(69, 168, 91, 0.5)', 'rgba(69, 168, 91, 1)'];
    else if (estimation >= 0.61 && estimation < 0.81) return ['rgba(219, 215, 101, 0.5)', 'rgba(219, 215, 101, 1)'];
    else if (estimation >= 0.41 && estimation < 0.61) return ['rgba(235, 161, 52, 0.5)', 'rgba(235, 161, 52, 1)'];
    return ['rgba(219, 66, 66, 0.5)', 'rgba(219, 66, 66, 1)'];
  }

  doughnutChartMethod() {
    this.doughnutChart = new Chart(this.doughnutCanvas?.nativeElement, {
      type: `doughnut`,
      data: {
        labels: [this.name],
        datasets: [
          {
            label: '',
            data: [this.result, this.remain],
            backgroundColor: [
              this.getColor(this.result / this.estimationFrom)[0],
              'rgba(200, 200, 200, 0.5)',
              // 'rgba(153, 102, 255, 0.2)',
              // 'rgba(255, 159, 64, 0.2)',
            ],
            borderColor: [
              this.getColor(this.result / this.estimationFrom)[1],
              'rgba(0, 0, 0, 0)',

              // 'rgba(153, 102, 255, 1)',
              // 'rgba(255, 159, 64, 1)',
            ],
            borderWidth: 2,
          },
        ],
      },
      options: {
        rotation: 0,
        circumference: 360,
        cutout: '70%',
        plugins: {
          legend: {
            display: false
          },
          tooltip: {
            enabled: true
          },
        }
      }
    });
  }
}
