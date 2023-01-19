var currentEvent;

var formatDate = function formatDate(date) {
    return date === null ? '' : moment(date).format("MM/DD/YYYY h:mm A");
};

var fpStartTime = flatpickr("#StartTime", {
    enableTime: true,
    dateFormat: "m/d/Y h:i K"
});
var fpEndTime = flatpickr("#EndTime", {
    enableTime: true,
    dateFormat: "m/d/Y h:i K"
});

$('#calendar').fullCalendar({
    
    defaultView: 'month',
    height: 'parent',
    header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
    },
    eventRender: function eventRender(event, $el) {
        $el.qtip({
            content: {
                title: event.title,
                text: event.description
                
            },
            hide: {
                event: 'unfocus'
            },
            show: {
                solo: true
            },
            position: {
                my: 'top left',
                at: 'bottom left',
                viewport: $('#calendar-wrapper'),
                adjust: {
                    method: 'shift'
                }
            }
        });
    },
    events: '/Calendar/GetCalendarEvents',
    eventClick: updateEvent,
    selectable: true,
    select: addEvent

    
});






/**
 * Calendar Methods
 **/

function updateEvent(event, element) {
    currentEvent = event;
    if ($(this).data("qtip")) $(this).qtip("hide");
    $('#eventModalLabel').html('Edit Event');
    $('#eventModalSave').html('Update Event');
    $('#EventTitle').val(event.title);
    $('#Description').val(event.description);
    $('#isNewEvent').val(false);
    var start = formatDate(event.start);
    var end = formatDate(event.end);
    fpStartTime.setDate(start);
    fpEndTime.setDate(end);
    $('#Start').val(start);
    $('#End').val(end);

    if (event.allDay) {
        $('#AllDay').prop('checked', 'checked');
    } else {
        $('#AllDay')[0].checked = false;
    }

    $('#eventModal').modal('show');
}

function addEvent(start, end) {
    $('#eventForm')[0].reset();

    $('#eventModalLabel').html('Add Event');
    $('#eventModalSave').html('Create Event');
    $('#isNewEvent').val(true);

    start = formatDate(start);
    end = formatDate(end);

    fpStartTime.setDate(start);
    fpEndTime.setDate(end);

    // Set the Start and End properties of the event object
    event.Start = start;
    event.End = end;

    $('#eventModal').modal('show');
}
/**
 * Modal
 * */


$('#eventModalSave').click(function () {
    var title = $('#EventTitle').val();
    var description = $('#Description').val();
    var startTime = moment($('#Start').val());
    var endTime = moment($('#End').val());
    var isAllDay = $('#AllDay').is(":checked");
    var isNewEvent = $('#isNewEvent').val() === 'true' ? true : false;

    if (startTime > endTime) {
        alert('Start Time cannot be greater than End Time');
        return;
    } else if ((!startTime.isValid() || !endTime.isValid()) && !isAllDay) {
        alert('Please enter both Start Time and End Time');
        return;
    }

    var event = {
        title: title,
        description: description,
        isAllDay: isAllDay,
        startTime: startTime._i,
        endTime: endTime._i
    };

    if (isNewEvent) {
        sendAddEvent(event);
    } else {
        sendUpdateEvent(event);
    }
});

function sendAddEvent(event) {
    console.log('Sending add event request');

    console.log(event.startTime);
    console.log(event.endTime);

    axios({
        method: 'post',
        url: '/Calendar/AddEvent',
        data: {
            "Title": event.title,
            "Description": event.description,
            "StartEvent": event.startTime,
            "EndEvent": event.endTime,
            "AllDay": event.isAllDay
        }
    }).then(res => {
        console.log('Received response from add event request');
        console.log(res.data);  // 2nd item
        const { message, eventId } = res.data;

        if (message === '') {
            const newEvent = {
                start: event.startTime,
                end: event.endTime,
                allDay: event.isAllDay,
                title: event.title,
                description: event.description,
                eventId
            };

            $('#calendar').fullCalendar('renderEvent', newEvent);
           
            $('#calendar').fullCalendar('unselect');
            
            $('#eventModal').modal('hide');

            window.location.href = '/Calendar/ListEvents';
        } else {
            alert(`Something went wrong: ${message}`);
        }
    }).catch(err => {
        console.log('Received error from add event request');
        console.log(err);  // 4th item
        alert(`Something went wrong: ${err}`);
    });
}


function sendUpdateEvent(event) {
    axios({
        method: 'post',
        url: '/Calendar/UpdateEvent',
        data: {
            "EventId": currentEvent.eventId,
            "Title": event.title,
            "Description": event.description,
            "StartEvent": event.startTime,
            "EndEvent": event.endTime,
            "AllDay": event.isAllDay
        }
    }).then(function (res) {
        var message = res.data.message;

        if (message === '') {
            currentEvent.title = event.title;
            currentEvent.description = event.description;
            currentEvent.start = event.startTime;
            currentEvent.end = event.endTime;
            currentEvent.allDay = event.isAllDay;
            $('#calendar').fullCalendar('updateEvent', currentEvent);
            $('#eventModal').modal('hide');
        } else {
            alert("Something went wrong: ".concat(message));
        }
    }).catch(function (err) {
        return alert("Something went wrong: ".concat(err));
    });
}

$('#deleteEvent').click(function () {
    if (confirm("Do you really want to delte \"".concat(currentEvent.title, "\" event?"))) {
        axios({
            method: 'post',
            url: '/Calendar/DeleteEvent',
            data: {
                "EventId": currentEvent.eventId
            }
        }).then(function (res) {
            var message = res.data.message;

            if (message === '') {
                $('#calendar').fullCalendar('removeEvents', currentEvent._id);
                $('#eventModal').modal('hide');
            } else {
                alert("Something went wrong: ".concat(message));
            }
        }).catch(function (err) {
            return alert("Something went wrong: ".concat(err));
        });
    }
});
$('#AllDay').on('change', function (e) {
    if (e.target.checked) {
        $('#EndTime').val('');
        fpEndTime.clear();
        this.checked = true;
    } else {
        this.checked = false;
    }
});
$('#EndTime').on('change', function () {
    $('#AllDay')[0].checked = false;
});

//# sourceURL=webpack:///./Scripts/calendar.js?