import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DateRequest } from './date-request';
import { environment } from '../../environments/environment';

@Component({
  selector: 'khk-date-test',
  templateUrl: './date-test.component.html',
  styleUrls: ['./date-test.component.scss']
})
export class DateTestComponent implements OnInit {
  url = `${environment.apiHost}/api/date`;

  request: DateRequest;
  response: any;

  inputDate: Date;
  inputDateText: string;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.inputDate = new Date();
    this.inputDateText = (new Date()).toISOString();
  }

  onSubmit() {
    if (!this.inputDate) {
      console.error('empty date!!!!');
      return;
    }
console.log(this.inputDate);

    const body = new DateRequest();
    const date = new Date(this.inputDate);
    const isoDate = date.toISOString();
    const utc = date.toUTCString();

    body.date = date;
    body.dateString = isoDate; // this.inputDateText;
    body.epoch = this.getEpoch(date);
    this.http.post<any>(this.url, body).subscribe(res => {
      this.response = res;
    });

console.log('submitted', body, utc);
  }

  private getEpoch(date: Date): number {
    return Math.round(date.getTime() / 1000);
  }
}
