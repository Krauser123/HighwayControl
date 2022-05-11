import moment from 'moment';
import React, { Component } from 'react';
import Dropdown from 'react-bootstrap/Dropdown'
import DropdownItem from 'react-bootstrap/esm/DropdownItem';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { SensorCreateInfo } from '../classes/SensorCreateInfo';
import { SensorData } from '../classes/SensorData';

interface HomeProps {

}

interface HomeState {
    dateValue: Date;
    sensorIdValue: number;
    sensorData: SensorData[];
    sensors: SensorCreateInfo[];
    sensorSelected: SensorCreateInfo | null;
}

export class Home extends Component<HomeProps, HomeState> {
    static displayName = Home.name;

    constructor(props: HomeProps) {
        super(props);
        this.state = { dateValue: new Date(), sensorIdValue: 1, sensorData: [], sensors: [], sensorSelected: null };
        this.onChange = this.onChange.bind(this);
        this.changeDdr = this.changeDdr.bind(this);

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

    changeDdr(eventKey: any, event: Object) {
        const selected = this.state.sensors.find(o => o.id == eventKey);
        if (selected != undefined) {
            this.setState({ sensorSelected: selected });
        }
    }

    render() {

        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-sm">
                            <Dropdown onSelect={this.changeDdr}>
                                <Dropdown.Toggle variant="success" id="dropdown-basic">
                                    {this.state.sensorSelected == null ? "Sensors" : this.state.sensorSelected.name}
                                </Dropdown.Toggle>
                                <Dropdown.Menu>
                                    {this.state.sensors.map(element => (
                                        <DropdownItem eventKey={element.id}>{element.name}</DropdownItem>
                                    ))}
                                </Dropdown.Menu>
                            </Dropdown>
                        </div>
                        <div className="col-sm">
                            <Calendar onChange={this.onChange} value={this.state.dateValue} />
                        </div>
                    </div>
                </div>

                <div className="container data">

                    <div className="row">
                        <div className="col-sm">
                            {"Car Plate"}
                        </div>
                        <div className="col-sm">
                            {"Car Speed"}
                        </div>
                        <div className="col-sm">
                            {"Date"}
                        </div>
                    </div>

                    {this.state.sensorData.map(element => (
                        <div className="row">
                            <div className="col-sm">
                                {element.carPlate}
                            </div>
                            <div className="col-sm">
                                {element.carSpeed}
                            </div>

                            <div className="col-sm">
                                {moment(element.catchDate).format('DD/MM/YYYY')}
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        );
    }
}
