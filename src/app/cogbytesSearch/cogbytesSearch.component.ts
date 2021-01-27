import { Component, OnInit, Inject, NgModule } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormControl, Validators, ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { NgModel } from '@angular/forms';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { ReplaySubject } from 'rxjs/ReplaySubject';
import { Subject } from 'rxjs/Subject';
import { take, takeUntil } from 'rxjs/operators';
import { ManageUserService } from '../services/manageuser.service';
import { PagerService } from '../services/pager.service';
import { DeleteConfirmDialogComponent } from '../delete-confirm-dialog/delete-confirm-dialog.component';
import { AuthorDialogeComponent } from '../authorDialoge/authorDialoge.component';
import { IdentityService } from '../services/identity.service';
import { CogbytesService } from '../services/cogbytes.service'
import { CogbytesSearch } from '../models/cogbytesSearch'
import { AuthenticationService } from '../services/authentication.service';
import { PubscreenDialogeComponent } from '../pubscreenDialoge/pubscreenDialoge.component';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';


@Component({
    selector: 'app-cogbytesSearch',
    templateUrl: './cogbytesSearch.component.html',
    styleUrls: ['./cogbytesSearch.component.scss']
})
export class CogbytesSearchComponent implements OnInit {



    authorModel: any;
    piModel: any;
    titleModel: any;
    abstractModel: any;
    yearModel: any;
    keywordsModel: any;
    doiModel: any;
    paperTypeModel: any;
    cognitiveTaskModel: any;
    specieModel: any;
    sexModel: any;
    strainModel: any;
    genoModel: any;
    ageModel: any;
    interventionModel: any;

    yearFromSearchModel: any;
    yearToSearchModel: any;
    authorMultiSelect: any;

    panelOpenState = false;

    // Definiing List Variables
    repList: any;

    paperTypeList: any;
    taskList: any;
    specieList: any;
    sexList: any;
    strainList: any;
    genoList: any;
    ageList: any

    authorList: any;
    authorList2: any;
    piList: any;
    searchResultList: any;
    yearList: any;
    paperInfoFromDoiList: any;
    checkYear: boolean;

    isAdmin: boolean;
    isUser: boolean;

    _cogbytesSearch = new CogbytesSearch;
    isSearch: boolean;
    filteredSearchList: any;

    //yearFrom = new FormControl('', []);
    yearTo = new FormControl('', []);

    public authorMultiFilterCtrl: FormControl = new FormControl();
    public filteredAutorList: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
    /** Subject that emits when the component has been destroyed. */
    private _onDestroy = new Subject<void>();

    dialogRef: MatDialogRef<DeleteConfirmDialogComponent>;

    constructor(public dialog: MatDialog,
        private authenticationService: AuthenticationService,
        private cogbytesService: CogbytesService,
        public dialogAuthor: MatDialog,
        private spinnerService: Ng4LoadingSpinnerService,) { }

    ngOnInit() {

        this.GetRepositories();
        this.GetAuthorList();
        this.GetPIList();
        this.cogbytesService.getTask().subscribe(data => { this.taskList = data; });
        this.cogbytesService.getSpecies().subscribe(data => { this.specieList = data; });
        this.cogbytesService.getSex().subscribe(data => { this.sexList = data; });
        this.cogbytesService.getStrain().subscribe(data => { this.strainList = data; });
        this.cogbytesService.getGenos().subscribe(data => { this.genoList = data; });
        this.cogbytesService.getAges().subscribe(data => { this.ageList = data; });

        this.isAdmin = this.authenticationService.isInRole("administrator");
        this.isUser = this.authenticationService.isInRole("user");
        this.yearList = this.GetYear(1970).sort().reverse();

        this.interventionModel = "All";

        this.isSearch = false;

    }

    ngOnDestroy() {
        this._onDestroy.next();
        this._onDestroy.complete();
    }

    // Function definition to get list of years
    GetYear(startYear) {
        var currentYear = new Date().getFullYear(), years = [];
        startYear = startYear || 1980;
        while (startYear <= currentYear) {
            years.push(startYear++);
        }
        return years;
    }


    GetAuthorList() {


        this.cogbytesService.getAuthor().subscribe(data => {
            this.authorList = data;

            // load the initial expList
            this.filteredAutorList.next(this.authorList.slice());

            this.authorMultiFilterCtrl.valueChanges
                .pipe(takeUntil(this._onDestroy))
                .subscribe(() => {
                    this.filterAuthor();
                });

        });

        return this.authorList;
    }


    GetPIList() {

        this.cogbytesService.getPI().subscribe(data => {
            this.piList = data;

            // load the initial expList
            //this.filteredPIList.next(this.piList.slice());

            //this.piMultiFilterCtrl.valueChanges
            //    .pipe(takeUntil(this._onDestroy))
            //    .subscribe(() => {
            //        this.filterPI();
            //    });

        });

        return this.piList;
    }

    // handling multi filtered Author list
    private filterAuthor() {
        if (!this.authorList) {
            return;
        }

        // get the search keyword
        let searchAuthor = this.authorMultiFilterCtrl.value;

        if (!searchAuthor) {
            this.filteredAutorList.next(this.authorList.slice());
            return;
        } else {
            searchAuthor = searchAuthor.toLowerCase();
        }

        // filter the Author
        this.filteredAutorList.next(
            this.authorList.filter(x => x.lastName.toLowerCase().indexOf(searchAuthor) > -1)
        );
    }

    setDisabledValSearch() {

        if
        (
            this.authorModel == null && this.piModel == null && this.titleModel == null && this.keywordsModel == null
            && this.doiModel == null && this.cognitiveTaskModel == null && this.specieModel == null && this.sexModel == null
            && this.strainModel == null && this.genoModel == null && this.ageModel == null
        )
        {
            return true;
        }

        return false;

    }

    selectYearToChange(yearFromVal, yearToVal) {
        console.log(yearToVal)
        yearFromVal = yearFromVal === null ? 0 : yearFromVal;
        
        yearToVal < yearFromVal ? this.yearTo.setErrors({ 'incorrect': true }) : false;

    }

    
    getErrorMessageYearTo() {
        //return this.yearTo.getError('Year To should be greater than Year From');
        return 'Year To should be greater than Year From'
    }


    GetRepositories() {
        this.cogbytesService.getAllRepositories().subscribe(data => { this.repList = data; });
        return this.repList;
    }

    // Function definition for searching publications based on search criteria
    search() {

        this._cogbytesSearch.authorID = this.authorModel;
        this._cogbytesSearch.piID = this.piModel;
        this._cogbytesSearch.repID = this.titleModel;
        this._cogbytesSearch.keywords = this.keywordsModel;
        this._cogbytesSearch.doi = this.doiModel;
        //this._pubSCreenSearch.year = this.yearModel;
        //this._pubSCreenSearch.years = this.yearSearchModel;
        this._cogbytesSearch.taskID = this.cognitiveTaskModel;
        this._cogbytesSearch.specieID = this.specieModel;
        this._cogbytesSearch.sexID = this.sexModel;
        this._cogbytesSearch.strainID = this.strainModel;
        this._cogbytesSearch.genoID = this.genoModel;
        this._cogbytesSearch.ageID = this.ageModel;

        this._cogbytesSearch.yearFrom = this.yearFromSearchModel;
        this._cogbytesSearch.yearTo = this.yearToSearchModel;

        this._cogbytesSearch.intervention = this.interventionModel;

        //console.log(this._cogbytesSearch);

        this.cogbytesService.searchRepositories(this._cogbytesSearch).subscribe(data => {

            this.searchResultList = data;
            console.log(this.searchResultList);
            this.isSearch = true;
        });
    }

    // Function to filter search list based on repository
    getFilteredSearchList(repID: number) {
        return this.searchResultList.filter(x => x.repID == repID);
    }

    // Function for getting string of repository authors
    getRepAuthorString(rep: any) {
        let authorString: string = "";
        for (let id of rep.authourID) {
            let firstName: string = this.authorList[this.authorList.map(function (x) { return x.id }).indexOf(id)].firstName;
            let lastName: string = this.authorList[this.authorList.map(function (x) { return x.id }).indexOf(id)].lastName;
            authorString += firstName + "-" + lastName + ", ";
        }
        return authorString.slice(0, -2);
    }

    // Function for getting string of repository PIs
    getRepPIString(rep: any) {
        let PIString: string = "";
        for (let id of rep.piid) {
            PIString += this.piList[this.piList.map(function (x) { return x.id }).indexOf(id)].piFullName + ", ";
        }
        return PIString.slice(0, -2);
    }
}


