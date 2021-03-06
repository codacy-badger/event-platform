import { Component, Input, ViewChild } from "@angular/core";
import { Store } from "@ngrx/store";
import { Event } from "../../../../../api/models/event";
import { EventFormDto } from "../../../../../components/event-form/dto/event-form.dto";
import { EventUtils } from "../../../../../utils/event.utils";
import { EventFormComponent } from "../../../../../components/event-form/event-form.component";
import { selectGeneralSettings, State } from "../../store/event-settings.reducer";
import { EditEvent } from "./store/general-settings.actions";

@Component({
    selector: "app-general-settings",
    templateUrl: "general-settings.template.html"
})
export class GeneralSettingsComponent {
    @ViewChild(EventFormComponent, { static: true })
    form: EventFormComponent;

    @Input()
    set event(event: Event) {
        this.eventFormDto = EventUtils.eventToEventFormDto(event);
    }

    loading$ = this.store$.pipe(selectGeneralSettings(x => x.loading));

    eventFormDto = new EventFormDto();

    constructor(private store$: Store<State>) {
    }

    clickSave() {
        if (this.form.validate()) {
            this.store$.dispatch(new EditEvent(this.eventFormDto));
        }
    }
}
