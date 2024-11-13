jQuery(document).ready(function(){
	Waves.init();	
	$('.data-table').DataTable();
	jQuery(".dataTable").wrap('<div class="table-responsive"></div>');
	
	$('.btn-toggle').click(function() {
		$(this).find('.btn').toggleClass('active');      
		if ($(this).find('.btn-primary').length>0) {
			$(this).find('.btn').toggleClass('btn-primary');
		}
		if ($(this).find('.btn-danger').length>0) {
			$(this).find('.btn').toggleClass('btn-danger');
		}
		if ($(this).find('.btn-success').length>0) {
			$(this).find('.btn').toggleClass('btn-success');
		}
		if ($(this).find('.btn-info').length>0) {
			$(this).find('.btn').toggleClass('btn-info');
		}
		
	   $(this).find('.btn').toggleClass('btn-default');       
	});
});



jQuery(document).on("click",".vertical-menu-btn", function (e) {
	e.preventDefault();
	if (jQuery(window).width() < 992) {
		jQuery("body").removeClass("vertical-collpsed");
		jQuery("body").toggleClass("sidebar-enable");
	}
	else{
		jQuery("body").toggleClass("sidebar-enable vertical-collpsed");
	}
});	


//new Chart(document.getElementById("totalallocation"), {
//	type: 'bar',
//	data: {
//		labels: ['jan','Feb','Mar', 'Apr', 'May', 'Jun'],
//		datasets: [{
//			label: 'Total Request',
//			borderColor: '#7770d0',
//			backgroundColor:'#7770d0',
//			data: [529, 236, 327,458,209,345]
//		}, {
//			label: 'Total allocation',
//			borderColor: '#f9a24f',
//			backgroundColor:'#f9a24f',
//			//backgroundColor:'rgba(246,246,247,0.3)',
//			data: [429, 236, 300,408,200,324]
//		}]
//	},
//	options: {
//		title: {
//		display: false,
//		text: ''
//		},
//		tooltips: {
//			mode: 'index',
//			intersect: false
//		},
//		responsive: true,
//		scales: {
//		xAxes: [{
//			stacked: false
//		}],
//		yAxes: [{
//			stacked: false
//		}]
//	},
//    plugins: {
//      datalabels: {
//        align: 'end',
//        anchor: 'end',
//        backgroundColor: function(context) {
//          return context.dataset.backgroundColor;
//        },
//        borderRadius: 4,
//        color: 'white',
//        formatter: function(value){
//            return value + ' (100%) ';
//        }
//      }
//    }
//  }
//});


/*

var ctx = document.getElementById("employeeUtilization");

// And for a doughnut chart
var myDoughnutChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
		labels: [ "Red", "Blue"],
		datasets: [{
			data:[300, 100],
			backgroundColor: ['#6259ca', '#e7e8ec'],
			hoverBackgroundColor: [ "#6259ca", "#e7e8ec"]
		}]	
	},
    options: {
    	rotation: 1 * Math.PI,
      circumference: 1 * Math.PI
    }
});
*/


//new Chart(document.getElementById("trackerallocation"), {
//	type: 'doughnut',
//	data: {
//		labels: ['Developer','UI','Testing','CUX'],
//		datasets: [
//	    {
//	      label: '',
//	      //data: [360, 500, 420],
//        data: [250, 1000, 500,120],
//      	borderWidth: 0,
//        backgroundColor: ['#6259ca', '#eb6f33', '#ec546c', '#0774f8', '#9857CD']
//    	},	
			
//		]
//	}
//});


