import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
<<<<<<< HEAD
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
=======
>>>>>>> 3177331ec521a1852e9fc4fa8d69a81eeefdb096

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'The Dating App';
  users: any;


<<<<<<< HEAD
  constructor(private accountService: AccountService){}

  ngOnInit(){
    this.setCurrentUsers();
  }

  setCurrentUsers(){
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  
=======
  constructor(private http: HttpClient){}

  ngOnInit(){
    this.getUsers(); 
  }

  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }
>>>>>>> 3177331ec521a1852e9fc4fa8d69a81eeefdb096
}


