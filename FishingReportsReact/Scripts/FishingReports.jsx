var ReportsMain = React.createClass({
 getInitialState: function() {
    return {data: []};
  },
 componentWillMount: function() {
    var xhr = new XMLHttpRequest();
    xhr.open('get', this.props.url, true);
    xhr.onload = function() {
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
  render: function() {
    return (
      <div className="reportsMain">
		<ReportsList data={this.state.data} clickHandler={this.displayReportDetails} />
      </div>
    );
  }
});

var ReportsList = React.createClass({
  render: function() {

  var reportNodes = this.props.data.map(function(report) {
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
					<th>Total Fish</th>
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
  render: function() {
    return (
		<tr key={this.props.report.ReportId}>
			<td>{this.props.report.ReportDateString}</td>
			<td>{this.props.report.Location}</td>
			<td>{this.props.report.Access}</td>
			<td>{this.props.report.TotalFish}</td>
            <td>{this.props.report.Username}</td>
            <td><a href="#" onClick={() => this.props.clickHandler(this.props.report.ReportId)}>Details</a></td>
		</tr>
    );
  }
});

var ReportDetails = React.createClass({
    returnToList: function() {
        ReactDOM.render(
            <ReportsMain url="reports" />,
            document.getElementById('content')
        );
    },
    render: function() {
        return (
            <div>
                <hr />
                <p>Showing details for Report: {this.props.reportId}</p>
                <a href="#" onClick={() => this.returnToList()}>Back to list</a>
            </div>
        );
    }
});

ReactDOM.render(
  <ReportsMain url="reports" />,
  document.getElementById('content')
);