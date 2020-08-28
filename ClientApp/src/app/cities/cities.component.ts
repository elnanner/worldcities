import { Component, OnInit, Inject, ViewChild, ChangeDetectorRef  } from '@angular/core';
import { City } from './city';
import { HttpClient, HttpParams } from '@angular/common/http';

import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ApiResult } from '../shared/api-result.model';


@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent implements OnInit {

  public displayedColumns: string[] = ['id', 'name', 'lat', 'lon'];
  public cities: MatTableDataSource<City>;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    const pageEvent = new PageEvent();
    pageEvent.pageIndex = 0;
    pageEvent.pageSize = 10;
    this.getData(pageEvent);
  }

  getData(event: PageEvent) {
    const url = this.baseUrl + 'api/Cities';
    let params = new HttpParams()
      .set('pageIndex', event.pageIndex.toString())
      .set('pageSize', event.pageSize.toString());

    this.http.get<ApiResult>(url, { params })
      .subscribe(result => {
        this.cities = new MatTableDataSource<City>(result.data);
        this.cdr.detectChanges();
        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;

      }, error => console.log(error));
  }
}
