import { Dropdown } from 'bootstrap';
import moment from 'moment';
import React, { Component } from 'react';
import { DropdownButton } from 'react-bootstrap';
import DropdownItem from 'react-bootstrap/esm/DropdownItem';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';

interface HomeProps {

}

interface HomeState {
    dateValue: Date;
    sensorIdValue: number;
    sensorData: [];
    sensors: [];
}

export class Home extends Component<HomeProps, HomeState> {
    static displayName = Home.name;

    constructor(props: HomeProps) {
        super(props);
        this.state = { dateValue: new Date(), sensorIdValue: 1, sensorData: [], sensors: [] };
        this.onChange = this.onChange.bind(this);
        this.getSensors();
    }

    async getSensorData() {
        let formated = moment(this.state.dateValue).format('DD/MM/YYYY');
        const response = await fetch("https://localhost:7269/" + "GetSensorsData?sensorId=" + this.state.sensorIdValue + "&date=" + formated, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();
        this.setState({ sensorData: data });
    }

    async getSensors() {
        const response = await fetch("https://localhost:7269/" + "GetSensors", {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();
        this.setState({ sensors: data });
    }

    onChange(nextValue: any) {
        this.setState({ dateValue: nextValue })
        this.getSensorData();
    }

    render() {
        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-sm">
                            <DropdownButton id="dropdown-basic-button" title="Dropdown button">
                                <DropdownItem href="#/action-1">item</DropdownItem>
                            </DropdownButton>
                        </div>
                        <div className="col-sm">
                            <Calendar onChange={this.onChange} value={this.state.dateValue} />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
