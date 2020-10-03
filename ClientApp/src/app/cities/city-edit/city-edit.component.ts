import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { City } from '../city';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.css']
})
export class CityEditComponent implements OnInit {

  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the city object to edit
  city: City;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl('')
    });
    this.loadData();
  }

  loadData() {
    // retrieve the ID from de 'id' parameter
    let id = +this.activatedRoute.snapshot.paramMap.get('id');


    // fetch the city from the server
    var url = this.baseUrl + 'api/cities/' + id;
    this.http.get<City>(url).subscribe(result => {
      this.city = result;
      this.title = 'Edit - ' + this.city.name;

      // update the form with the city value
      this.form.patchValue(this.city);
    }, error => console.error(error));
  }


  onSubmit() {
    let city = this.city;

    city.name = this.form.get('name').value;
    city.lat = this.form.get('lat').value;
    city.lon = this.form.get('lon').value;

    let url = this.baseUrl + 'api/cities/' + this.city.id;
    this.http
      .put<City>(url, city)
      .subscribe(result => {
        console.log('City ' + city.id + ' has been updated. ');

        // go back to cities view
        this.router.navigate(['/cities']);
      }, error => console.log(error));
  }
}
