import { Component, Inject } from "@nestjs/common";
import { Model } from "mongoose";
import { Template } from "./templates.model";
import { CreateTemplateDto, UpdateTemplateDto } from "./templates.dto";

@Component()
export class TemplatesService {
    constructor(@Inject("TemplatesModelToken") private readonly templatesModel: Model<Template>) {
    }

    async create(createTemplateDto: CreateTemplateDto) {
        const template = new this.templatesModel(createTemplateDto);
        return template.save();
    }

    async findAll(): Promise<Template[]> {
        return this.templatesModel.find().exec();
    }

    async findOne(name: string): Promise<Template> {
        return this.templatesModel.findOne({ name: name }).exec();
    }

    async update(name: string, updateTemplateDto: UpdateTemplateDto) {
        return this.templatesModel.update({ name: name }, updateTemplateDto).exec();
    }

    async remove(name: string) {
        return this.templatesModel.remove({ name: name }).exec();
    }
}
