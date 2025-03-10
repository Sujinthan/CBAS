import { Component, OnInit, Inject, NgModule } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormControl, Validators, ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { NgModel } from '@angular/forms';
import { Request } from '../models/request';
import { RequestService } from '../services/request.service';
import { SharedModule } from '../shared/shared.module';



@Component({

    selector: 'app-reqMouseLineDialoge',
    templateUrl: './reqMouseLineDialoge.component.html',
    styleUrls: ['./reqMouseLineDialoge.component.scss'],
    providers: [RequestService]

})
export class ReqMouseLineDialogeComponent implements OnInit {

    // Defining Models Parameters

    reqNameModel: string;
    reqEmailModel: string;
    reqStrainModel: string;
    reqGenoModel: string;
    geneticModificationModel: string
    refModel: string
    controlModel: string
   
   // DEfining obj model parameters        
    private _request = new Request();

    // FormControl Parameters

    name = new FormControl('', [Validators.required]);
    email = new FormControl('', [Validators.required, Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")]);
    strain = new FormControl('', [Validators.required]);
    geneticModi = new FormControl('', [Validators.required]);
    //control = new FormControl('', [Validators.required]);
    

    // List of GeneticModification for Mouse line
    public geneticModification: any[] = [
           
        { name: 'Transgenic' },
        { name: 'WildType' },
        { name: 'KnockIn' },
        { name: 'KnockOut' }
        
    ];
    

    constructor(public thisDialogRef: MatDialogRef<ReqMouseLineDialogeComponent>,
         
        private requestService: RequestService, ) { }

    ngOnInit() {
      
    }

    onCloseCancel(): void {


        this.thisDialogRef.close('Cancel');

    }

    onCloseSubmit(): void {

        // building request object
        this._request.fullName = this.reqNameModel;
        this._request.email = this.reqEmailModel;
        this._request.mouseStrain = this.reqStrainModel;
        this._request.genotype = this.reqGenoModel;
        this._request.geneticModification = this.geneticModificationModel;
        this._request.strainReference = this.refModel;
        this._request.controlSuggestion = this.controlModel;

        
        // Submiting the request to server
        this.requestService.addMouseLine(this._request).subscribe( this.thisDialogRef.close()); 
       
    }


    getErrorMessage()
    {

        return this.name.hasError('required') ? 'You must enter a value' :
            '';
    }

    getErrorMessageEmail()
    {

        return this.email.hasError('required') ? 'You must enter a value' :
            '';

    }
    
    getErrorMessageEmailValid()
    {

        return this.email.hasError('pattern') ? 'Enter Valid Email Address' :
            '';
    }

    getErrorMessageStrain() {
        return this.strain.hasError('required') ? 'You must enter a value' :
            '';
    }

    getErrorMessageGM() {

        return this.geneticModi.hasError('required') ? 'You must enter a value' :
            '';
    }

    //getErrorMessageControl() {
    //    return this.control.hasError('required') ? 'You must enter a value' :
    //        '';
    //}

      
    setDisabledVal()
    {

        if (this.name.hasError('required') ||
            this.email.hasError('required') ||
            this.email.hasError('pattern') ||
            this.strain.hasError('required') ||
            this.geneticModi.hasError('required')
                                   
        )

        {
            return true;
        }

        return false;
    }

        
}
