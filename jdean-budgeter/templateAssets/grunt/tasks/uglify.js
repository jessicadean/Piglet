/*Uglify*/

module.exports = function(grunt, data){
	
	return {
		dist: {
			"options":{
				"mangle": true,
				"preserveComments": false,
				"banner": "<%= banner %>"
			},
			"files": {
	      "<%= path.dist %>/html/assets/js/app.min.js": ["<%= path.dist %>/html/assets/js/app.js"],
	      "<%= path.dist %>/light/assets/js/app.min.js": ["<%= path.dist %>/light/assets/js/app.js"]
	    }
	  }
  };
};