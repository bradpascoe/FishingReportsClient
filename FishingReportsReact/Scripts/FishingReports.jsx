var ReportsMain = React.createClass({
    getInitialState: function () {
        return { data: [] };
    },
    componentWillMount: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    displayReportDetails(value) {
        ReactDOM.render(
            <ReportDetails reportId={value} url="details" />,
            document.getElementById('content'));
    },
    render: function () {
        return (
      <div className="reportsMain">
		<ReportsList data={this.state.data} clickHandler={this.displayReportDetails} />
      </div>
    );
    }
});

var ReportsList = React.createClass({
    render: function () {

        var reportNodes = this.props.data.map(function (report) {
            return (
        <ReportListItem report={report} clickHandler={this.props.clickHandler} key={report.ReportId}>
        </ReportListItem>
      );
        }.bind(this));
        return (
      <div className="reportsList">
		<table id="t01">
			<thead>
				<tr>
					<th>Date</th>
					<th>Location</th>
					<th>Access</th>
                    <th>Type</th>
					<th>Total Fish</th>
                    <th>Images</th>
                    <th>Flow (cfs)</th>
                    <th>Username</th>
				</tr>
			</thead>
			<tbody>{reportNodes}</tbody>
		</table>
      </div>
    );
    }
});

var ReportListItem = React.createClass({
    render: function () {
        return (
		<tr key={this.props.report.ReportId}>
			<td>{this.props.report.ReportDateString}</td>
			<td>{this.props.report.Location}</td>
			<td>{this.props.report.Access}</td>
            <td>{this.props.report.ReportType}</td>
			<td>{this.props.report.TotalFish}</td>
            <td>{this.props.report.NumberOfImages}</td>
            <td>{this.props.report.FlowRate}</td>
            <td>{this.props.report.Username}</td>
            <td><a href="#" onClick={() => this.props.clickHandler(this.props.report.ReportId)}>Details</a></td>
		</tr>
    );
    }
});

var ReportImage = React.createClass({
    render: function () {
        return (
		    <p>
                <a href={this.props.reportImage.ImageName} target="_blank">
                    <img src={this.props.reportImage.ThumbnailName}/>
                </a>
		    </p>
        );
    }
});

var ReportDetails = React.createClass({
    getInitialState: function () {
        return { data: [] };
    },
    componentWillMount: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + "/" + this.props.reportId, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    returnToList: function () {
        ReactDOM.render(
            <ReportsMain url="reports" />,
            document.getElementById('content')
        );
    },
    render: function () {

        console.log(this.state);
        var reportImages = null;

        if (this.state.data.Images) {
            reportImages = this.state.data.Images.map(function (image) {
                return (
                    <ReportImage reportImage={image} key={image.ImageId}>
                    </ReportImage>
                );
            }.bind(this));
        }

        return (
            <div>
                <h4>{this.state.data.ReportDateString}</h4>
                <p>{this.state.data.Location} - {this.state.data.Access}, {this.state.data.State}</p>
                <p>Submitted by {this.state.data.Username}</p>
                <hr />
                <p>Report type: {this.state.data.ReportType}</p>
                <p>Stream flow: {this.state.data.FlowRate}</p>
                <p>Water conditions: {this.state.data.WaterConditions}</p>
                <p>Weather conditions: {this.state.data.WeatherConditions}</p>
                <p>Fish landed: {this.state.data.TotalFish}</p>
                <p>{this.state.data.Description}</p>
                {reportImages}
                <p><a href="#" onClick={() => this.returnToList()}>Back to list</a></p>
            </div>
        );
    }
});

ReactDOM.render(
  <ReportsMain url="reports" />,
  document.getElementById('content')
);