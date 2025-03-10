import { Component, OnInit, Inject, NgModule } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
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
import { AuthenticationService } from '../services/authentication.service';
import { CogbytesDialogueComponent } from '../cogbytesDialogue/cogbytesDialogue.component';
import { CogbytesUpload } from '../models/cogbytesUpload'
import { CogbytesService } from '../services/cogbytes.service'
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { NotificationDialogComponent } from '../notification-dialog/notification-dialog.component';


@Component({
    selector: 'app-cogbytes',
    templateUrl: './cogbytes.component.html',
    styleUrls: ['./cogbytes.component.scss']
})


export class CogbytesComponent implements OnInit {

    readonly DATASET = 1;
    public uploadKey: number;
    panelOpenState: boolean;
    dialogRefLink: MatDialogRef<NotificationDialogComponent>;
    showGeneratedLink: boolean;
    public repModel: any;

    // Definiing List Variables
    repList: any;
    uploadList: any;
    authorList: any;
    piList: any;

    _cogbytesUpload = new CogbytesUpload();


    isAdmin: boolean;
    isUser: boolean;
    isFullDataAccess: boolean;

    /** Subject that emits when the component has been destroyed. */
    private _onDestroy = new Subject<void>();

    dialogRef: MatDialogRef<DeleteConfirmDialogComponent>;

    constructor(
        public dialog: MatDialog,
        private authenticationService: AuthenticationService,
        //public dialogAuthor: MatDialog,
        private cogbytesService: CogbytesService,
        private spinnerService: Ng4LoadingSpinnerService,
    )
    {
        this.resetFormVals();
    }

    ngOnInit() {
        this.panelOpenState = false;


        this.isAdmin = this.authenticationService.isInRole("administrator");
        this.isUser = this.authenticationService.isInRole("user");
        this.isFullDataAccess = this.authenticationService.isInRole("fulldataaccess");

        if (this.isAdmin || this.isUser) {

            this.cogbytesService.getRepositories().subscribe(data => { this.repList = data; console.log(data)});
            this.GetAuthorList();
            this.GetPIList();
        }
    }


    ngOnDestroy() {
        this._onDestroy.next();
        this._onDestroy.complete();
    }

    resetFormVals() {

    }

    GetRepositories() {
        this.GetAuthorList();
        this.GetPIList();
        this.cogbytesService.getRepositories().subscribe(data => { this.repList = data; });
        //return this.repList;
    }

    GetUploads() {
        if (this.repModel != null) {
            let repID = this.getRep().id;
            this.cogbytesService.getUploads(repID).subscribe(data => { this.uploadList = data; });
        }
    }

    ClosePanel() {
        this.panelOpenState = false;
    }

    NewUpload() {
        this.GetUploads();
        this.ClosePanel();
    }


    //// Opening Dialog for adding a new repository.
    openDialogAddRepository(): void {
        let dialogref = this.dialog.open(CogbytesDialogueComponent, {
            height: '850px',
            width: '1200px',
            data: {
                repObj: null,
            }

        });

        dialogref.afterClosed().subscribe(result => {
            console.log('the dialog was closed');
            this.repModel = null;
            this.GetRepositories();
           
        });
    }

    //// Opening Dialog for editing an existing repository.
    openDialogEditRepository(): void {
        let dialogref = this.dialog.open(CogbytesDialogueComponent, {
            height: '850px',
            width: '1200px',
            data: {
                repObj: this.repList[this.repList.map(function (x) { return x.id }).indexOf(this.repModel)],
            }

        });

        dialogref.afterClosed().subscribe(result => {
            console.log('the dialog was closed');
            this.GetRepositories();
        });
    }

    // Delete File Dialog
    deleteRepository(file) {
        this.dialogRef = this.dialog.open(DeleteConfirmDialogComponent, {
            disableClose: false
        });
        this.dialogRef.componentInstance.confirmMessage = "Are you sure you want to delete this repository and all associated uploads and files?"

        this.dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.spinnerService.show();
                this.cogbytesService.deleteRepository(this.getRep().id).map(res => {

                }).subscribe();
                this.spinnerService.hide();
                this.repModel = null;
            }
            this.dialogRef = null;
        });
    }

    getRepID() : number {
        return this.repList[this.repList.map(function (x) { return x.id }).indexOf(this.repModel)].id;
    }


    GetAuthorList() {


        this.cogbytesService.getAuthor().subscribe(data => {
            this.authorList = data;
        });

        return this.authorList;
    }


    GetPIList() {

        this.cogbytesService.getPI().subscribe(data => {
            this.piList = data;
        });

        return this.piList;
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

    getRep(): any {
        return this.repList[this.repList.map(function (x) { return x.id }).indexOf(this.repModel)];
    }

    // Get Guid by RepoID
    getLink(repID) {

        this.cogbytesService.getGuidByRepID(repID).subscribe(data => {

            this.showGeneratedLink = true;
            var guid = data.repoLinkGuid;

            this.dialogRefLink = this.dialog.open(NotificationDialogComponent, {
            });
            this.dialogRefLink.componentInstance.message = "http://localhost:4200/comp-edit?repolinkguid=" + guid;

        });

    }

    getLinkURL() {
        return "http://localhost:4200/comp-edit?repolinkguid=" + this.getRep().repoLinkGuid;
    }

}


