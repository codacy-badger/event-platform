import { Required } from "src/app/form-generator/decorators/required.decorator";
import { Control } from "src/app/form-generator/decorators/control.decorator";
import { Min } from "src/app/form-generator/decorators/min.decorator";
import { Max } from "src/app/form-generator/decorators/max.decorator";

export class SponsorPositionningDto {
    @Control()
    @Required()
    @Min(0)
    @Max(1)
    webWidth: number;

    @Control()
    @Required()
    @Min(0)
    @Max(1)
    webHeight: number;

    @Control()
    @Required()
    webPadding: number[];

    @Control()
    @Required()
    @Min(0)
    @Max(1)
    mobileWidth: number;

    @Control()
    @Required()
    @Min(0)
    @Max(1)
    mobileHeight: number;

    @Control()
    @Required()
    mobilePadding: number[];
}
