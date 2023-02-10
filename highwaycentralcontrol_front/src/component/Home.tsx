import moment from 'moment';
import React, { Component } from 'react';
import Dropdown from 'react-bootstrap/Dropdown'
import DropdownItem from 'react-bootstrap/esm/DropdownItem';
import 'react-calendar/dist/Calendar.css';
import { SensorCreateInfo } from '../classes/SensorCreateInfo';
import { SensorData } from '../classes/SensorData';

interface HomeProps {

}

interface HomeState {
    sensorIdValue: number;
    sensorData: SensorData[];
    sensors: SensorCreateInfo[];
    sensorSelected: SensorCreateInfo | null;
}

export class Home extends Component<HomeProps, HomeState> {
    static displayName = Home.name;

    constructor(props: HomeProps) {
        super(props);
        this.state = { sensorIdValue: 1, sensorData: [], sensors: [], sensorSelected: null };
        this.changeDdr = this.changeDdr.bind(this);

        this.getSensors();
    }

    async getSensorData(sensorSelectedId: number) {
        const response = await fetch("http://localhost:7999/" + "GetSensorsData?sensorId=" + sensorSelectedId, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();
        this.setState({ sensorData: data });
    }

    async getSensors() {
        const response = await fetch("http://localhost:7999/" + "GetSensors", {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();
        this.setState({ sensors: data });
    }

    changeDdr(eventKey: any, event: Object) {
        const selected = this.state.sensors.find(o => o.id == eventKey);
        if (selected != undefined) {
            this.getSensorData(selected.id);
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

                    {this.state.sensorData.map((element, index) => (
                        <div className="row" key={index}>
                            <div className="col-sm">
                                {element.carPlate}
                            </div>
                            <div className="col-sm">
                                {element.carSpeed}
                            </div>
                            <div className="col-sm">
                                {moment(element.dataDate).format('DD/MM/YYYY')}
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        );
    }
}
